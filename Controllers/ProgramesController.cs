﻿
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
public class ProgramesController(IProgramesServices programesServices) : ControllerBase
{
    private readonly IProgramesServices programesServices = programesServices;

    [HttpGet("Recomend-places")]
    public async Task<IActionResult> RecomendPlaces( CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracts ID
        var result = await programesServices.RecomendPlaces( userId!, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("Trip-Details")]
    public async Task<IActionResult> TripDetails(string TripName ,  CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracts ID
        var result = await programesServices.TripDetails(userId!, TripName, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("DisplayAllTrips")]
    public async Task<IActionResult> DisplayAllTrips(CancellationToken cancellationToken)
    { 
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracts ID
        var result = await programesServices.AllTripsInProgram( userId! ,cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("All-TripsName")]
    [AllowAnonymous]
    public async Task<IActionResult> AllTripsName(CancellationToken cancellationToken)
    {
        var result = await programesServices.AllTripsName(cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

}
