using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Tourism_Api.Pagnations;
using Tourism_Api.Services.IServices;

namespace Tourism_Api.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[EnableRateLimiting(RateLimiters.Concurrency)]
public class PlaceController(IPlaceService placeService)
    : ControllerBase
{
    private readonly IPlaceService placeService = placeService;
    [HttpGet("DisplayAllPlaces")]
    public async Task<IActionResult> DisplayAllPlaces(CancellationToken cancellationToken)
    {
        var result = await placeService.DisplayAllPlaces(cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet("DisplayAllPlacesByPagnation")]
    public async Task<IActionResult> DisplayAllPlacesByPagnation([FromQuery] RequestFilters requestFilters, CancellationToken cancellationToken)
    {
        var result = await placeService.DisplayAllPlacesByPagnation(requestFilters, cancellationToken);
        return Ok(result);
    }
    [HttpGet("PlacesDetails")]
    public async Task<IActionResult> PlacesDetails([FromQuery] string name, CancellationToken cancellationToken)
    {
        var result = await placeService.PlacesDetails(name, cancellationToken);
        return result is not null ? Ok(result) : NotFound();
    }
    [HttpGet("All-PlacesName")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPlacesName(CancellationToken cancellationToken)
    {
        var result = await placeService.AllPlacesName(cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
