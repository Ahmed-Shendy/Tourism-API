using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Tourism_Api.Entity.Comment;
using Tourism_Api.Entity.Places;
using Tourism_Api.Entity.user;
using Tourism_Api.Outherize;
using Tourism_Api.Services.IServices;

namespace Tourism_Api.Controllers;
[Route("api/[controller]")]
[ApiController]
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Authorize(Roles = DefaultRoles.Member, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

public class UserController (IUserServices userServices , IHttpContextAccessor httpContextAccessor) : ControllerBase
{
    private readonly IUserServices userServices = userServices;
    private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;

    [HttpPost("AddComment")]
    public async Task<IActionResult> AddComment(AddComment request , CancellationToken cancellationToken)
    {
        // var userId = User.fin(ClaimTypes.NameIdentifier); // Extracts ID
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracts ID

        var result = await userServices.AddComment(userId! ,request , cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpDelete("DeleteComment")]
    public async Task<IActionResult> DeleteComment(int CommentId, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracts ID
        var result = await userServices.DeleteComment(userId!, CommentId, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPut("UpdateComment")]
    public async Task<IActionResult> UpdateComment(UpdateComment request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracts ID
        var result = await userServices.UpdateComment(userId!, request, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    [HttpPost("Addrate")]
    public async Task<IActionResult> Addrate(AddRate request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracts ID
        var result = await userServices.Addrate(userId!, request, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPost("ReservationTourguid")]
    public async Task<IActionResult> ReservationTourguid(string TourguidId, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracts ID
        var result = await userServices.ReservationTourguid(userId!, TourguidId, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpDelete("CancelReservationTourguid")]
    public async Task<IActionResult> CancelReservationTourguid(CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracts ID
        var result = await userServices.CancelReservationTourguid(userId!, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpGet("DisplayReservationTourguid")]
    public async Task<IActionResult> DisplayReservationTourguid(CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracts ID
        var result = await userServices.DisplayReservationTourguid(userId!, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    
    [HttpGet("UserProfile")]
    public async Task<IActionResult> UserProfile(CancellationToken cancellationToken)
    {
        
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracts ID
  
        var result = await userServices.UserProfile(userId!, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("PublicProfile")]
    [AllowAnonymous]
    public async Task<IActionResult> PublicProfile(string userId, CancellationToken cancellationToken)
    {
        var result = await userServices.PublicProfile(userId, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPut("UpdateProfile")]
    public async Task<IActionResult> UpdateProfile(ProfileUpdate request, CancellationToken cancellationToken)
    {
         var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracts ID
        //var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
    
        //string userId = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var result = await userServices.UpdateProfile(userId!, request, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPost("AddOrRemoveFavoritePlace")]
    public async Task<IActionResult> AddFavoritePlace(string PlaceName, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracts ID
        var result = await userServices.AddOrRemoveFavoritePlace(userId!, PlaceName, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    // [HttpDelete("RemoveFavoritePlace")]
    // public async Task<IActionResult> RemoveFavoritePlace(string PlaceName, CancellationToken cancellationToken)
    // {
    //     var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracts ID
    //     var result = await userServices.RemoveFavoritePlace(userId!, PlaceName, cancellationToken);
    //     return result.IsSuccess ? Ok() : result.ToProblem();
    // }
    [HttpPost("AddTourguidRate")]
    public async Task<IActionResult> AddTourguidRate(AddTourguidRate request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracts ID
        var result = await userServices.AddTourguidRate(userId!, request, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }


}
