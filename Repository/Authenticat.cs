using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;
using Tourism_Api.Entity.user;
using Tourism_Api.model;
using Tourism_Api.model.Context;
using Tourism_Api.Outherize;

namespace Tourism_Api.Repository;

public class Authenticat(TourismContext _db, token token,
    UserManager<User> user
    , SignInManager<User> signInManager
    , ILogger<Authenticat> logger
    )
    
{
    private readonly TourismContext db = _db;
    private readonly token Token = token;
    private readonly UserManager<User> _userManager = user;
    private readonly SignInManager<User> signInManager = signInManager;
    private readonly ILogger<Authenticat> logger = logger;
    private readonly int RefreshTokenDays = 14;


    public async Task<UserRespones> RegisterAsync(UserRequest userRequest, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(userRequest.Email))
        {
            logger.LogError("Email is required.");
            return null!;
        }

        var emailIsExists = await db.Users.AnyAsync(x => x.Email == userRequest.Email, cancellationToken);
        if (emailIsExists)
            return new UserRespones { Name = "Email must unique" } ;

        var request = userRequest.Adapt<User>();
        request.UserName = request.Email;
        
        var save = await _userManager.CreateAsync(request, request.Password);
        await db.SaveChangesAsync(cancellationToken);
        if (save.Succeeded)
        {
          var result = db.Users.SingleOrDefault(x => x.Email == userRequest.Email).Adapt<UserRespones>();
            var (token, expiresIn) = Token.GenerateToken(result);
            result.Token = token;
            result.ExpiresIn = expiresIn;

            result.RefreshToken = GenerateRefreshToken();
            result.RefreshTokenExpiretion = DateTime.UtcNow.AddDays(RefreshTokenDays);

            request.RefreshTokens.Add(new RefreshToken
            {
                Token = result.RefreshToken,
                ExpiresOn = result.RefreshTokenExpiretion,
            });
            await db.SaveChangesAsync(cancellationToken);
            
            return result;
        }

        var error = save.Errors.ToList();
        logger.LogError("User creation failed: {error}", error[0].Description);

        return null!;

    }

    public async Task<UserRespones> LoginAsync(userLogin userLogin, CancellationToken cancellationToken = default)
    {
        

        var user = await db.Users.SingleOrDefaultAsync(i => i.Email == userLogin.Email, cancellationToken);

        if (user is null)
            return null!;

        var isValidPassword = await signInManager.PasswordSignInAsync(user, userLogin.Password, false, true);
 

        if (user.LockoutEnd != null)
        {
            if (user.LockoutEnd > DateTime.UtcNow)
                return new UserRespones { Name = "Looked user For 5 Minutes" };
        }
        if (!isValidPassword.Succeeded)
            return null!;
        var result = user.Adapt<UserRespones>();
        var (token, expiresIn) = Token.GenerateToken(result);
        result.Token = token;
        result.ExpiresIn = expiresIn;
        result.RefreshToken = GenerateRefreshToken();
        result.RefreshTokenExpiretion = DateTime.UtcNow.AddDays(RefreshTokenDays);
        user.RefreshTokens.Add(new RefreshToken
        {
            Token = result.RefreshToken,
            ExpiresOn = result.RefreshTokenExpiretion,
        });
        await db.SaveChangesAsync(cancellationToken);
        return result;
    }

    public async Task<UserRespones> GetRefreshToken( UserRefreshToken request , CancellationToken cancellationToken = default)
    {

        var UserEmail = Token.ValisationToken(request.Token);

        if (UserEmail is null)
            return null!;

        var user = await db.Users.SingleOrDefaultAsync(i=> i.Email == UserEmail, cancellationToken);

        if (user is null)
            return null!;

        var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == request.RefrehToken && x.IsActive);

        if (userRefreshToken is null)
            return null!;

        // ***to stop Refresh token
        userRefreshToken.RevokedOn = DateTime.UtcNow;

        var result = user.Adapt<UserRespones>();
        var (newtoken, expiresIn) = Token.GenerateToken(result);
        result.Token = newtoken;
        result.ExpiresIn = expiresIn;

        result.RefreshToken = GenerateRefreshToken();
        result.RefreshTokenExpiretion = DateTime.UtcNow.AddDays(RefreshTokenDays);

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = result.RefreshToken,
            ExpiresOn = result.RefreshTokenExpiretion
        });
        await db.SaveChangesAsync(cancellationToken);
        await _userManager.UpdateAsync(user);

        return result;
    }

    private static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(count: 64));
    }
}
