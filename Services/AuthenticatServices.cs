using Hangfire;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using Tourism_Api.model.Context;
using Tourism_Api.Outherize;
using Tourism_Api.Services.IServices;

namespace Tourism_Api.Services;

public class AuthenticatServices(TourismContext _db, token token,
    UserManager<User> user
    , SignInManager<User> signInManager
    , ILogger<AuthenticatServices> logger
    , IEmailService emailService
    , IHttpContextAccessor httpContextAccessor
    ) : IAuthenticatServices

{
    private readonly TourismContext db = _db;
    private readonly token Token = token;
    private readonly UserManager<User> _userManager = user;
    private readonly SignInManager<User> signInManager = signInManager;
    private readonly ILogger<AuthenticatServices> logger = logger;
    private readonly IEmailService emailSender = emailService;
    private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;

    private readonly int RefreshTokenDays = 14;


    public async Task<Result<UserRespones>> RegisterAsync(UserRequest userRequest, CancellationToken cancellationToken = default)
    {


        var emailIsExists = await db.Users.AnyAsync(x => x.Email == userRequest.Email, cancellationToken);
        if (emailIsExists)
            return Result.Failure<UserRespones>(UserErrors.EmailUnque);

        var request = userRequest.Adapt<User>();
        request.UserName = request.Email;
        request.EmailConfirmed = false;
        request.ConfirmCode = GenerateEmailConfirmationToken();
        var save = await _userManager.CreateAsync(request, request.Password);
       // await db.SaveChangesAsync(cancellationToken);
        if (save.Succeeded)
        {
            await _userManager.AddToRoleAsync(request, DefaultRoles.Member);
            var confirmationLink = BuildConfirmationLink(request.Id, request.ConfirmCode!);

            var emailBody = BuildConfirmationEmailBody(request.Name, confirmationLink);

            BackgroundJob.Enqueue(() => emailSender.SendEmailAsync(
                request.Email!,
                "Confirm your Egypt_Guid account",
                emailBody));

            var result = request.Adapt<UserRespones>();
            result.Role = DefaultRoles.Member;
            result.Message = "Registration completed. Please confirm your email from the message we sent you before logging in.";

          //  await db.SaveChangesAsync(cancellationToken);

            return Result.Success(result);
        }

        var error = save.Errors.ToList();
        logger.LogError("User creation failed: {error}", error[0].Description);

        return Result.Failure<UserRespones>(UserErrors.notsaved);

    }

    public async Task<EmailConfirmationViewModel> ConfirmEmailAsync(string userId, string token, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
            return CreateConfirmationViewModel(
                isSuccess: false,
                title: "Invalid confirmation link",
                message: "The confirmation link is incomplete. Please request a new one and try again.");

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return CreateConfirmationViewModel(
                isSuccess: false,
                title: "Account not found",
                message: "We could not find an account for this confirmation link.");

        if (user.EmailConfirmed)
            return CreateConfirmationViewModel(
                isSuccess: true,
                title: "Email already confirmed",
                message: "Your email has already been confirmed. You can go back and log in to EDuSmart.ai now.");

        if (!string.Equals(user.ConfirmCode, token, StringComparison.Ordinal))
        {
            return CreateConfirmationViewModel(
                isSuccess: false,
                title: "Confirmation failed",
                message: "We could not confirm your email with this link. Please request a new confirmation email and try again.");
        }

        user.EmailConfirmed = true;
        user.ConfirmCode = null;

        var updateResult = await _userManager.UpdateAsync(user);

        if (!updateResult.Succeeded)
        {
            logger.LogWarning(
                "Email confirmation update failed for user {UserId}. Errors: {Errors}",
                userId,
                string.Join(", ", updateResult.Errors.Select(error => error.Description)));

            return CreateConfirmationViewModel(
                isSuccess: false,
                title: "Confirmation failed",
                message: "Your email was verified, but we could not save the confirmation right now. Please try again.");
        }

        await db.SaveChangesAsync(cancellationToken);

        return CreateConfirmationViewModel(
            isSuccess: true,
            title: "Email confirmed successfully",
            message: "Your email is confirmed now. You can log in to EDuSmart.ai whenever you want.");
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
        var emailAttribute = new System.ComponentModel.DataAnnotations.EmailAddressAttribute();
        if (!emailAttribute.IsValid(Email))
        {
            return Result.Failure(new Error("InvalidEmail", "The provided email address is not valid.", 400));
        }

        var user = await db.Users.SingleOrDefaultAsync(i => i.Email == Email, cancellationToken);
        if (user != null)
        {
            Random random = new Random();
            var code = random.Next(100000, 999999);
            user.ConfirmCode = code.ToString();
            string body = $"Your password reset code is: {code}\n\nPlease use this code to reset your password. This code is valid for a limited time.";
           
            //await emailSender.SendEmailAsync(Email, "Reset Password", body);

            //use background job to send email
            BackgroundJob.Enqueue(() => emailSender.SendEmailAsync(Email, "Reset Password",body));
            
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

    private string BuildConfirmationLink(string userId, string confirmationToken)
    {
        var request = httpContextAccessor.HttpContext?.Request;
        var baseUrl = request is not null
            ? $"{request.Scheme}://{request.Host}"
            : "https://localhost:7214";

        return QueryHelpers.AddQueryString(
            $"{baseUrl}/auth/confirm-email",
            new Dictionary<string, string?>
            {
                ["userId"] = userId,
                ["token"] = confirmationToken
            });
    }

    private static string BuildConfirmationEmailBody(string userName, string confirmationLink)
    {
        return $"""
            <!DOCTYPE html>
            <html lang="en">
            <head>
                <meta charset="UTF-8" />
                <meta name="viewport" content="width=device-width, initial-scale=1.0" />
                <title>Confirm your Egypt_Guid account</title>
            </head>
            <body style="margin:0;padding:32px;background:#f4efe7;font-family:Segoe UI,Tahoma,Arial,sans-serif;color:#1f2937;">
                <table role="presentation" style="max-width:640px;width:100%;margin:0 auto;background:#ffffff;border-radius:24px;overflow:hidden;box-shadow:0 20px 60px rgba(15,23,42,.12);border:1px solid #eadfce;">
                    <tr>
                        <td style="padding:32px 32px 16px;background:linear-gradient(135deg,#0f766e,#14532d);color:#ffffff;">
                            <div style="font-size:14px;letter-spacing:.2em;text-transform:uppercase;opacity:.85;">EDuSmart.ai</div>
                            <h1 style="margin:14px 0 8px;font-size:30px;line-height:1.2;">Confirm your email</h1>
                            <p style="margin:0;font-size:16px;line-height:1.8;opacity:.92;">Welcome {System.Net.WebUtility.HtmlEncode(userName)}. One click is all you need to activate your account.</p>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding:32px;">
                            <p style="margin:0 0 18px;font-size:16px;line-height:1.8;">Please confirm your email address to complete your registration and start using EDuSmart.ai.</p>
                            <p style="margin:0 0 28px;">
                                <a href="{confirmationLink}" style="display:inline-block;padding:14px 28px;border-radius:999px;background:#0f766e;color:#ffffff;text-decoration:none;font-weight:700;">Confirm Email</a>
                            </p>
                            <p style="margin:0 0 10px;font-size:14px;line-height:1.8;color:#6b7280;">If the button does not work, copy and paste this link into your browser:</p>
                            <p style="margin:0;font-size:14px;line-height:1.8;word-break:break-all;color:#0f766e;">{confirmationLink}</p>
                        </td>
                    </tr>
                </table>
            </body>
            </html>
            """;
    }

    private static EmailConfirmationViewModel CreateConfirmationViewModel(bool isSuccess, string title, string message)
    {
        return new EmailConfirmationViewModel
        {
            IsSuccess = isSuccess,
            Title = title,
            Message = message,
            Icon = isSuccess ? "check" : "close",
            ActionText = isSuccess ? "Back to login" : null
        };
    }

    private static string GenerateEmailConfirmationToken()
    {
        return Guid.NewGuid().ToString("N");
    }
}
