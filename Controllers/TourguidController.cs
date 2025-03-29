using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tourism_Api.Entity.Tourguid;
using Tourism_Api.Services.IServices;

namespace Tourism_Api.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = DefaultRoles.Tourguid, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

public class TourguidController(ITourguidService tourguidService) : ControllerBase
{
    private readonly ITourguidService tourguidService = tourguidService;

    [HttpGet("Profile")]
    public async Task<IActionResult> Profile(CancellationToken cancellationToken = default)
    {
        var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracts ID

        var result = await tourguidService.Profile( id! , cancellationToken);
        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }
    [HttpPost("update-profile")]
    public async Task<IActionResult> UpdateProfile(TourguidUpdateProfile request, CancellationToken cancellationToken = default)
    {
        var id = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var result = await tourguidService.UpdateProfile(id!, request, cancellationToken);
        return result.IsSuccess
            ? Ok()
            : result.ToProblem();
    }
    [HttpDelete("RemoveTourist")]
    public async Task<IActionResult> RemoveTourist(string Touristid, CancellationToken cancellationToken = default)
    {
        var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await tourguidService.RemoveTourist(id!, Touristid, cancellationToken);
        return result.IsSuccess
            ? Ok()
            : result.ToProblem();
    }
}
