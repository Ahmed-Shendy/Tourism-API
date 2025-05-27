using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;
using Tourism_Api.model.Context;
using Tourism_Api.Outherize;
using Tourism_Api.Services.IServices;

namespace Tourism_Api.Services;

public class AuthenticatServices(TourismContext _db, token token,
    UserManager<User> user
    , SignInManager<User> signInManager
    , ILogger<AuthenticatServices> logger
    , IEmailService emailService
    ) : IAuthenticatServices

{
    private readonly TourismContext db = _db;
    private readonly token Token = token;
    private readonly UserManager<User> _userManager = user;
    private readonly SignInManager<User> signInManager = signInManager;
    private readonly ILogger<AuthenticatServices> logger = logger;
    private readonly IEmailService emailSender = emailService;



    private readonly int RefreshTokenDays = 14;


    public async Task<Result<UserRespones>> RegisterAsync(UserRequest userRequest, CancellationToken cancellationToken = default)
    {


        var emailIsExists = await db.Users.AnyAsync(x => x.Email == userRequest.Email, cancellationToken);
        if (emailIsExists)
            return Result.Failure<UserRespones>(UserErrors.EmailUnque);

        var request = userRequest.Adapt<User>();
        request.UserName = request.Email;
        request.EmailConfirmed = true;
        var save = await _userManager.CreateAsync(request, request.Password);
        await db.SaveChangesAsync(cancellationToken);
        if (save.Succeeded)
        {

            await db.SaveChangesAsync(cancellationToken);
            var user = await db.Users.SingleOrDefaultAsync(x => x.Email == userRequest.Email);
            var result = user.Adapt<UserRespones>();
            await _userManager.AddToRoleAsync(request, DefaultRoles.Member);
            var userRoles = (await _userManager.GetRolesAsync(request)).ToList();

            var (token, expiresIn) = Token.GenerateToken(result, userRoles);
            result.Token = token;
            result.Role = userRoles[0];
            result.ExpiresIn = expiresIn;
            result.RefreshToken = GenerateRefreshToken();
            result.RefreshTokenExpiretion = DateTime.UtcNow.AddDays(RefreshTokenDays);
            user!.RefreshTokens.Add(new RefreshToken
            {
                Token = result.RefreshToken,
                ExpiresOn = result.RefreshTokenExpiretion,
            });
            await db.SaveChangesAsync(cancellationToken);

            return Result.Success(result);
        }

        var error = save.Errors.ToList();
        logger.LogError("User creation failed: {error}", error[0].Description);

        return Result.Failure<UserRespones>(UserErrors.notsaved);

    }

    public async Task<Result<UserRespones>> LoginAsync(userLogin userLogin, CancellationToken cancellationToken = default)
    {

        var user = await db.Users.SingleOrDefaultAsync(i => i.Email == userLogin.Email, cancellationToken);

        if (user is null)
            return Result.Failure<UserRespones>(UserErrors.UserNotFound);

        var isValidPassword = await signInManager.PasswordSignInAsync(user, userLogin.Password, false, true);


        if (user.LockoutEnd != null)
        {
            if (user.LockoutEnd > DateTime.UtcNow)
                return Result.Failure<UserRespones>(UserErrors.LookUser);
            // return new UserRespones { Name = "Looked user For 5 Minutes" };
        }
        if (!isValidPassword.Succeeded)
            return Result.Failure<UserRespones>(UserErrors.UserNotFound);
        if (user.EmailConfirmed == false)
            return Result.Failure<UserRespones>(UserErrors.EmailNotConfirmed);


        var result = user.Adapt<UserRespones>();

        var userRoles = (await _userManager.GetRolesAsync(user)).ToList();

        var (token, expiresIn) = Token.GenerateToken(result, userRoles);
        result.Token = token;
        result.Role = userRoles[0];
        result.ExpiresIn = expiresIn;
        result.RefreshToken = GenerateRefreshToken();
        result.RefreshTokenExpiretion = DateTime.UtcNow.AddDays(RefreshTokenDays);
        user.RefreshTokens.Add(new RefreshToken
        {
            Token = result.RefreshToken,
            ExpiresOn = result.RefreshTokenExpiretion,
        });
        await db.SaveChangesAsync(cancellationToken);
        return Result.Success(result);
    }

    public async Task<Result<UserRespones>> GetRefreshToken(UserRefreshToken request, CancellationToken cancellationToken = default)
    {

        var UserEmail = Token.ValisationToken(request.Token);

        if (UserEmail is null)
            return Result.Failure<UserRespones>(UserErrors.UserNotFound);


        var user = await db.Users.SingleOrDefaultAsync(i => i.Email == UserEmail, cancellationToken);

        if (user is null)
            return Result.Failure<UserRespones>(UserErrors.UserNotFound);


        var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == request.RefrehToken && x.IsActive);

        if (userRefreshToken is null)
            return Result.Failure<UserRespones>(UserErrors.InvalidToken);

        // ***to stop Refresh token
        userRefreshToken.RevokedOn = DateTime.UtcNow;

        var result = user.Adapt<UserRespones>();
        var userRoles = (await _userManager.GetRolesAsync(user)).ToList();

        var (newtoken, expiresIn) = Token.GenerateToken(result, userRoles);
        result.Token = newtoken;
        result.ExpiresIn = expiresIn;
        result.Role = userRoles[0];
        result.RefreshToken = GenerateRefreshToken();
        result.RefreshTokenExpiretion = DateTime.UtcNow.AddDays(RefreshTokenDays);

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = result.RefreshToken,
            ExpiresOn = result.RefreshTokenExpiretion
        });
        await db.SaveChangesAsync(cancellationToken);
        await _userManager.UpdateAsync(user);

        return Result.Success(result);
    }

    public async Task<Result> ForGetPassword(string Email, CancellationToken cancellationToken)
    {
        var user = await db.Users.SingleOrDefaultAsync(i => i.Email == Email, cancellationToken);
        if (user != null)
        {
            Random random = new Random();
            var code = random.Next(100000, 999999);
            user.ConfirmCode = code.ToString();
            await emailSender.SendEmailAsync(Email, "Reset Password",
             $"Your password reset code is: {code}\n\nPlease use this code to reset your password. This code is valid for a limited time.");
            await db.SaveChangesAsync(cancellationToken);
        }
        return Result.Success();
    }


    public async Task<Result> GetCode(string code , CancellationToken cancellationToken )
    {
        var user = await db.Users.FirstOrDefaultAsync(i => i.ConfirmCode == code, cancellationToken);
        if (user == null)
            return Result.Failure(UserErrors.InvalidCode);
        // If the code is valid, you can proceed with further actions, like resetting the password.
       
         return Result.Success();
       
    }


    public async Task<Result> ResetPassword(ResetPasswordRequest resetPasswordRequest, CancellationToken cancellationToken)
    {

        var user = await db.Users.SingleOrDefaultAsync
            (i => i.Email == resetPasswordRequest.Email && i.ConfirmCode == resetPasswordRequest.ConfirmCode, cancellationToken);

        if (user == null)
            return Result.Failure(UserErrors.UserNotFound);

        // إنشاء رمز إعادة تعيين كلمة المرور
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        // إعادة تعيين كلمة المرور باستخدام Token
        var result = await _userManager.ResetPasswordAsync(user, token, resetPasswordRequest.NewPassword);

        if (!result.Succeeded)
        {
            // في حالة فشل عملية إعادة التعيين
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Result.Failure(UserErrors.notsaved);
        }

        // تحديث الحقول الأخرى
        user.ConfirmCode = null;
        user.LockoutEnd = null;
        user.LockoutEnabled = false;
        user.AccessFailedCount = 0;
        user.EmailConfirmed = true;

        await _userManager.UpdateAsync(user);

        return Result.Success();
    }




    private static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(count: 64));
    }
}
