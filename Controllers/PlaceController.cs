using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;
using Tourism_Api.Pagnations;
using Tourism_Api.Services.IServices;

namespace Tourism_Api.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[EnableRateLimiting(RateLimiters.Concurrency)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
[Produces("application/json")]
public class PlaceController(IPlaceService placeService)
    : ControllerBase
{
    private readonly IPlaceService placeService = placeService;
    [AllowAnonymous]
    [HttpGet("DisplayAllPlaces")]
    [ProducesResponseType(typeof(All_Places), StatusCodes.Status200OK)]
    public async Task<IActionResult> DisplayAllPlaces(CancellationToken cancellationToken)
    {
        var result = await placeService.DisplayAllPlaces(cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [AllowAnonymous]
    [HttpGet("DisplayAllPlacesByPagnation")]
    [ProducesResponseType(typeof(PaginatedList<ALLPlaces>), StatusCodes.Status200OK)]
    public async Task<IActionResult> DisplayAllPlacesByPagnation([FromQuery] RequestFilters requestFilters, CancellationToken cancellationToken)
    {
        var result = await placeService.DisplayAllPlacesByPagnation(requestFilters, cancellationToken);
        return Ok(result);
    }
    [HttpGet("PlacesDetails")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PlacesDetails), StatusCodes.Status200OK)]
    public async Task<IActionResult> PlacesDetails([FromQuery] string name, CancellationToken cancellationToken)
    {
        string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracts ID

        var result = await placeService.PlacesDetails(userId! ,name, cancellationToken);
        // return result is not null ? Ok(result) : NotFound();
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();

    }
    [HttpGet("All-PlacesName")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPlacesName(CancellationToken cancellationToken)
    {
        var result = await placeService.AllPlacesName(cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("SearchForPlace")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(List<ALLPlaces>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchForPlace([FromQuery] string name, CancellationToken cancellationToken)
    {
        var result = await placeService.SearchForPlace(name, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
