﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Tourism_Api.Entity.user;
using Tourism_Api.Services;
using Tourism_Api.Services.IServices;

namespace Tourism_Api.Controllers;
[Route("api/[controller]")]
[ApiController]
[EnableRateLimiting(RateLimiters.IpLimiter)]
public class authenticatController(
    IAuthenticatServices authenticat
    ) : ControllerBase
{
    private readonly IAuthenticatServices authenticat = authenticat;

    [HttpPost("Register")]
    public async Task<IActionResult> RegisterAsync(UserRequest request, CancellationToken cancellationToken = default)
    {
        var result = await authenticat.RegisterAsync(request, cancellationToken);
        //if (result == null)
        //    return BadRequest();
        //else if (result.Name == "Email must unique")
        //    return Conflict("Email must unique");
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpPost("Login")]
    public async Task<IActionResult> LoginAsync(userLogin request, CancellationToken cancellationToken = default)
    {
        var result = await authenticat.LoginAsync(request, cancellationToken);
        //if (result == null)
        //    return NotFound();
        //else if (result.Name == "Looked user For 5 Minutes")
        //    return Conflict("Looked user For 5 Minutes");
        //return Ok(result);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();


    }
    [HttpPost("GetRefreshToken")]
    public async Task<IActionResult> GetRefreshTokenAsync(UserRefreshToken request, CancellationToken cancellationToken = default)
    {
        var result = await authenticat.GetRefreshToken(request, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();


    }
    [HttpPost("ForGetPassword")]
    public async Task<IActionResult> ForGetPasswordAsync(string Email, CancellationToken cancellationToken = default)
    {
        var result = await authenticat.ForGetPassword(Email, cancellationToken);
        return result.IsSuccess ? Ok("Email sent") : result.ToProblem();
    }
    [HttpPost("GetCode")]
    public async Task<IActionResult> GetCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        var result = await authenticat.GetCode(code, cancellationToken);
        return result.IsSuccess ? Ok("Code sent") : result.ToProblem();
    }
    [HttpPost("ResetPassword")]
    public async Task<IActionResult> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken cancellationToken = default)
    {
        var result = await authenticat.ResetPassword(request, cancellationToken);
        return result.IsSuccess ? Ok("Password reset") : result.ToProblem();
    }
}
