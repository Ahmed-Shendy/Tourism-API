using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Tourism_Api.Outherize;

namespace Tourism_Api.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly TourismContext db;
        private readonly token Token;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthController(UserManager<User> userManager, IConfiguration config, IHttpClientFactory httpClientFactory, TourismContext db, token token = null)
        {
            _userManager = userManager;
            _config = config;
            _httpClientFactory = httpClientFactory;
            this.db = db;
            Token = token;
        }

        // 1️⃣ Initiate Google OAuth Flow
        [HttpGet("google-login")]
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(GoogleCallback), "Auth", null, Request.Scheme)
            };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            var authResult = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);

            if (!authResult.Succeeded || authResult.Principal is null)
                return BadRequest("Google authentication failed.");

            var email = authResult.Principal.FindFirstValue(ClaimTypes.Email);
            var googleId = authResult.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
            var name = authResult.Principal.FindFirstValue(ClaimTypes.Name);

            if (string.IsNullOrWhiteSpace(email))
                return BadRequest("Email not provided by Google.");

            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                var randomPassword = $"{Guid.NewGuid().ToString("N")}{Guid.NewGuid().ToString("N")}A12";
                user = new User
                {
                    Name = ResolveGoogleDisplayName(name, email),
                    UserName = email,
                    Email = email,
                    GoogleId = googleId,
                    EmailConfirmed = true,
                    Password = randomPassword
                };

                var createResult = await _userManager.CreateAsync(user, randomPassword);

                if (!createResult.Succeeded)
                    return BadRequest(createResult.Errors);

                await _userManager.AddToRoleAsync(user, DefaultRoles.Member);
            }
            else if (string.IsNullOrWhiteSpace(user.GoogleId) && !string.IsNullOrWhiteSpace(googleId))
            {
                user.GoogleId = googleId;
                await _userManager.UpdateAsync(user);
            }

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            return Ok(Result.Success(await BuildGoogleLoginResponseAsync(user)));
        }

        [HttpPost("google")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginDto dto)
        {
            // 1️⃣ تحقق من الـ ID Token مع جوجل
            var payload = await VerifyGoogleTokenAsync(dto.IdToken);
            if (payload == null)
                return Unauthorized("Invalid Google token");

            // 2️⃣ ابحث عن اليوزر أو أنشئه
            var user = await _userManager.FindByEmailAsync(payload.Email);
            if (user == null)
            {
                var randomPassword = $"{Guid.NewGuid().ToString("N")}{Guid.NewGuid().ToString("N")}A12";
                user = new User
                {
                    Name = ResolveGoogleDisplayName(payload.Name, payload.Email),
                    UserName = payload.Email,
                    Email = payload.Email,
                    GoogleId = payload.Subject,
                    EmailConfirmed = true,
                    Password = randomPassword
                };

                await _userManager.CreateAsync(user, randomPassword);
                await _userManager.AddToRoleAsync(user, DefaultRoles.Member);
            }

            return Ok(Result.Success(await BuildGoogleLoginResponseAsync(user)));
        }

        private async Task<GoogleJsonWebSignature.Payload> VerifyGoogleTokenAsync(string idToken)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { _config["Google:ClientId"] }
                };
                return await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Token validation failed: {ex.Message}");
                return null;
            }
        }

        private async Task<UserRespones> BuildGoogleLoginResponseAsync(User user)
        {
            var userResponse = user.Adapt<UserRespones>();
            var userRoles = (await _userManager.GetRolesAsync(user)).ToList();
            var (token, expiresIn) = Token.GenerateToken(userResponse, userRoles);

            userResponse.Token = token;
            userResponse.Role = userRoles.FirstOrDefault() ?? DefaultRoles.Member;
            userResponse.ExpiresIn = expiresIn;

            return userResponse;
        }

        private static string ResolveGoogleDisplayName(string? googleName, string email)
        {
            if (!string.IsNullOrWhiteSpace(googleName))
                return googleName;

            var emailPrefix = email.Split('@', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();

            return string.IsNullOrWhiteSpace(emailPrefix) ? "Google User" : emailPrefix;
        }
    }

    public class GoogleLoginDto
    {
        public string IdToken { get; set; }
    }

    //// 2️⃣ Handle Google Callback & Register/Login User
    //[HttpGet("signin-google")]
    //public async Task<IActionResult> GoogleCallback()
    //{

    //    var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
    //    if (!result.Succeeded)
    //        return BadRequest("Google authentication failed.");

    //    var email = result.Principal.FindFirstValue(ClaimTypes.Email);
    //    var googleId = result.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
    //    var name = result.Principal.FindFirstValue(ClaimTypes.Name);

    //    if (string.IsNullOrEmpty(email))
    //        return BadRequest("Email not provided by Google.");

    //    var user = await _userManager.FindByEmailAsync(email);

    //    if (user == null)
    //    {
    //        // 👤 REGISTER NEW USER
    //        user = new User
    //        {
    //            UserName = email,
    //            Email = email,
    //            GoogleId = googleId,
    //            EmailConfirmed = true // Google already verified email
    //        };

    //        // Identity requires a password. Use a secure random string since auth is handled by Google.
    //        var randomPassword = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");
    //        var createResult = await _userManager.CreateAsync(user, randomPassword);

    //        if (!createResult.Succeeded)
    //            return BadRequest("User registration failed.");
    //    }

    //    // 🔑 Generate JWT

    //    var finleresult = user.Adapt<UserRespones>();

    //    var userRoles = (await _userManager.GetRolesAsync(user)).ToList();

    //    var (token, expiresIn) = Token.GenerateToken(finleresult, userRoles);
    //    finleresult.Token = token;
    //    finleresult.Role = userRoles[0];
    //    finleresult.ExpiresIn = expiresIn;
    //    finleresult.RefreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(count: 64));
    //    finleresult.RefreshTokenExpiretion = DateTime.UtcNow.AddDays(17);
    //    user.RefreshTokens.Add(new RefreshToken
    //    {
    //        Token = finleresult.RefreshToken,
    //        ExpiresOn = finleresult.RefreshTokenExpiretion,
    //    });
    //    await db.SaveChangesAsync();
    //    return Ok(Result.Success(finleresult));


    //  // var token = GenerateJwtToken(user);
    //  //  return Ok(new { Token = token, Email = user.Email, Registered = user.CreationDate == DateTime.UtcNow });
    //}
}

