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
using Tourism_Api.Services;
using Tourism_Api.Services.IServices;

namespace Tourism_Api.Controllers;
[Route("api/[controller]")]
[ApiController]
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Authorize(Roles = DefaultRoles.Member, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
[Produces("application/json")]
public class UserController (IUserServices userServices ,
    IHttpContextAccessor httpContextAccessor ,
    EgyptGuideService egyptGuideService
    ) : ControllerBase
{
    private readonly IUserServices userServices = userServices;
    private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;

    // Injecting EgyptGuideService to access external API for places
    private readonly EgyptGuideService _egyptGuideService = egyptGuideService;

    [HttpPost("AddComment")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> AddComment(AddComment request , CancellationToken cancellationToken)
    {
        // var userId = User.fin(ClaimTypes.NameIdentifier); // Extracts ID
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracts ID

        var result = await userServices.AddComment(userId! ,request , cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpDelete("DeleteComment")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteComment(int CommentId, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracts ID
        var result = await userServices.DeleteComment(userId!, CommentId, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPut("UpdateComment")]  
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateComment(UpdateComment request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracts ID
        var result = await userServices.UpdateComment(userId!, request, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    [HttpPost("Addrate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Addrate(AddRate request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracts ID
        var result = await userServices.Addrate(userId!, request, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPost("ReservationTourguid")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ReservationTourguid(string TourguidId, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracts ID
        var result = await userServices.ReservationTourguid(userId!, TourguidId, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpDelete("CancelReservationTourguid")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelReservationTourguid(CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracts ID
        var result = await userServices.CancelReservationTourguid(userId!, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpGet("DisplayReservationTourguid")]
    [ProducesResponseType(typeof(Tourguids), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DisplayReservationTourguid(CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracts ID
        var result = await userServices.DisplayReservationTourguid(userId!, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    
    [HttpGet("UserProfile")]
    [ProducesResponseType(typeof(Profile), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UserProfile(CancellationToken cancellationToken)
    {
        
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracts ID
  
        var result = await userServices.UserProfile(userId!, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("PublicProfile")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(Public_Profile), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PublicProfile(string userId, CancellationToken cancellationToken)
    {
        var result = await userServices.PublicProfile(userId, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPut("UpdateProfile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateProfile(ProfileUpdate request, CancellationToken cancellationToken)
    {
         var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracts ID
        //var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
    
        //string userId = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var result = await userServices.UpdateProfile(userId!, request, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPost("AddOrRemoveFavoritePlace")]
    [ProducesResponseType(StatusCodes.Status200OK)]

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
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> AddTourguidRate(AddTourguidRate request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracts ID
        var result = await userServices.AddTourguidRate(userId!, request, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpGet("user-Program")]
    [ProducesResponseType(typeof(string),StatusCodes.Status200OK)]
    public async Task<IActionResult> RecommendProgram(CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracts ID
        var result = await userServices.RecomendProgram(userId!, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    //[HttpGet]
    //[Route("GetAllPlaces")]
    //[AllowAnonymous]
    //[ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
    //public async Task<IActionResult> GetAllPlaces()
    //{
    //    var places = await _egyptGuideService.GetAllPlacesAsync();
    //    return Ok(places);
    //}

    //[HttpPost("login")]
    //[AllowAnonymous]
    //public async Task<IActionResult> Login([FromBody] LoginRequest request)
    //{
    //    try
    //    {
    //        var response = await _egyptGuideService.LoginAsync(request.Email, request.Password);
    //        return Ok(response);
    //    }
    //    catch (HttpRequestException ex)
    //    {
    //        return BadRequest(new { message = ex.Message });
    //    }
    //}
    //[HttpGet("details/{placeName}")]
    //[AllowAnonymous]
    //public async Task<IActionResult> GetPlaceDetails(string placeName)
    //{
    //    try
    //    {
    //        // الحصول على الـ Token من رأس الطلب
    //        var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

    //        if (string.IsNullOrEmpty(token))
    //        {
    //            return Unauthorized("Authorization token is required");
    //        }

    //        var details = await _egyptGuideService.GetPlaceDetailsAsync(placeName, token);
    //        return Ok(details);
    //    }
    //    catch (UnauthorizedAccessException ex)
    //    {
    //        return Unauthorized(ex.Message);
    //    }
    //    catch (Exception ex)
    //    {
    //        return StatusCode(500, ex.Message);
    //    }
    //}

}
