using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;
using Tourism_Api.Entity.Tourguid;
using Tourism_Api.Entity.upload;
using Tourism_Api.model;
using Tourism_Api.Services.IServices;

namespace Tourism_Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TourguidController(ITourguidService tourguidService) : ControllerBase
{
    private readonly ITourguidService tourguidService = tourguidService;


    [HttpGet("Profile")]
    [Authorize(Roles = DefaultRoles.Tourguid, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public async Task<IActionResult> Profile( CancellationToken cancellationToken = default)
    {
        
        var id = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracts ID

        var result = await tourguidService.Profile( id! , cancellationToken);
        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }
    [HttpPut("update-profile")]
    [Authorize(Roles = DefaultRoles.Tourguid, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public async Task<IActionResult> UpdateProfile(TourguidUpdateProfile request, CancellationToken cancellationToken = default)
    {
        var id = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var result = await tourguidService.UpdateProfile(id!, request, cancellationToken);
        return result.IsSuccess
            ? Ok()
            : result.ToProblem();
    }
    [HttpDelete("RemoveTourist")]
    [Authorize(Roles = DefaultRoles.Tourguid, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> RemoveTourist(string Touristid, CancellationToken cancellationToken = default)
    {
        var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await tourguidService.RemoveTourist(id!, Touristid, cancellationToken);
        return result.IsSuccess
            ? Ok()
            : result.ToProblem();
    }
    [HttpGet("PublicProfile")]
    [Authorize( AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> PublicProfile(string id , CancellationToken cancellationToken = default)
    {
        var result = await tourguidService.PublicProfile(id, cancellationToken);
        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }
    [HttpPost("UploadPhoto")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> UploadPhoto([FromForm] UploadImageRequest photo, CancellationToken cancellationToken = default)
    {
        //var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracts ID
        
        var result = await tourguidService.UploadPhoto(id!, photo.Image , cancellationToken);
        return result.IsSuccess
            ? CreatedAtAction(nameof(Download), new { userid = id }, null)
            : result.ToProblem();
    }
    [HttpDelete("DeletePhoto")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> DeletePhoto(CancellationToken cancellationToken = default)
    {
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracts ID
        var result = await tourguidService.DeletePhoto(id!, cancellationToken);
        return result.IsSuccess
            ? Ok()
            : result.ToProblem();
    }
    [HttpPut("MoveToPlaces")]
    [Authorize(Roles = DefaultRoles.Tourguid , AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> MoveToPlaces(string placeName, CancellationToken cancellationToken = default)
    {
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracts ID
        var result = await tourguidService.MoveToPlaces(id!, placeName, cancellationToken);
        return result.IsSuccess
            ? Ok()
            : result.ToProblem();
    }
    [HttpGet("Download")]
    //[Authorize( AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Download(string? userid , CancellationToken cancellationToken = default)
    {
        string id;
        if (userid is null)
        {
          id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!; // Extracts ID
        }
        else
        {
            id = userid;
        }
        var result = await tourguidService.DownloadAsync( id , cancellationToken);
        return result.fileContent is [] ? NotFound() :  File(result.fileContent, result.ContentType , result.fileName);
    }
}
