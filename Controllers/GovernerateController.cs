using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Tourism_Api.Pagnations;
using Tourism_Api.Services.IServices;

namespace Tourism_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [EnableRateLimiting(RateLimiters.Concurrency)]
    public class GovernerateController(IGovernorateService governorateService) : ControllerBase
    {
        private readonly IGovernorateService _governorateService = governorateService;
        [HttpGet("All-Governorate-Pagnation")]
        public async Task<IActionResult> GetGovernoratePgnation([FromQuery]RequestFilters requestFilters , CancellationToken cancellationToken)
        {
            var result = await _governorateService.GetGovernoratePagnation(requestFilters , cancellationToken);
            //return Ok(result.Value);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();

        }
        [HttpGet("All-Governorate")]
        public async Task<IActionResult> GetGovernorate( CancellationToken cancellationToken)
        {
            var result = await _governorateService.GetGovernorate(  cancellationToken);
            return Ok(result.Value);
        }
        [HttpGet("GovernorateAndPlaces-pagnation")]
        public async Task<IActionResult> GetGovernorateAndPlacesPagnation([FromQuery] RequestFiltersScpical requestFilters ,  CancellationToken cancellationToken)
        {
            var result = await _governorateService.GetGovernorateAndPlacesAsync(requestFilters, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        [HttpGet("GovernorateAndPlaces")]
        public async Task<IActionResult> GetGovernorateAndPlaces(string name , CancellationToken cancellationToken)
        {
            var result = await _governorateService.GetGovernorateAndPlace(name, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpGet("All-GovernoratesName")]
        [AllowAnonymous]
        public async Task<IActionResult> GetGovernoratesName(CancellationToken cancellationToken)
        {
            var result = await _governorateService.GetGovernoratesName(cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
    }
}
