
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;
using Tourism_Api.Services.IServices;

namespace Tourism_Api.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize( AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[EnableRateLimiting(RateLimiters.Concurrency)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
[Produces("application/json")]
public class ProgramesController(IProgramesServices programesServices) : ControllerBase
{
    private readonly IProgramesServices programesServices = programesServices;

    [HttpGet("Recomend-places")]
    [ProducesResponseType(typeof(List<ALLPlaces>), StatusCodes.Status200OK)]
    public async Task<IActionResult> RecomendPlaces( CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracts ID
        var result = await programesServices.RecomendPlaces( userId!, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("Trip-Details")]
    [ProducesResponseType(typeof(TripDetails), StatusCodes.Status200OK)]
    public async Task<IActionResult> TripDetails(string TripName ,  CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracts ID
        var result = await programesServices.TripDetails(userId!, TripName, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("DisplayAllTrips")]
    [ProducesResponseType(typeof(List<TripsResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> DisplayAllTrips(CancellationToken cancellationToken)
    { 
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracts ID
        var result = await programesServices.AllTripsInProgram( userId! ,cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("All-TripsName")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> AllTripsName(CancellationToken cancellationToken)
    {
        var result = await programesServices.AllTripsName(cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

}
