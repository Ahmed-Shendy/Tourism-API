using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Tourism_Api.Entity.TypeOfTourism;
using Tourism_Api.Pagnations;
using Tourism_Api.Services.IServices;

namespace Tourism_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [EnableRateLimiting(RateLimiters.Concurrency)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public class TypeOfTourismController(ITypeOfTourismService typeOfTourismService) : ControllerBase
    {
        private readonly ITypeOfTourismService _typeOfTourismService = typeOfTourismService;
        [HttpGet("All-TypeOfTourism")]
        [ProducesResponseType(typeof(IEnumerable<TypeOfTourismResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllTypeOfTourism(CancellationToken cancellationToken)
        {
            var result = await _typeOfTourismService.GetAllTypeOfTourismAsync(cancellationToken);
            return Ok(result.Value);
        }
        [HttpGet("TypeOfTourismAndPlaces-pagnation")]
        [ProducesResponseType(typeof(PaginatedList<TypeOfTourismALLPlaces>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTypeOfTourismAndPlaces( [FromQuery] RequestFiltersScpical requestFilters, CancellationToken cancellationToken)
        {
            var result = await _typeOfTourismService.GetTypeOfTourismAndPlacesAsync(requestFilters, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        [HttpGet("TypeOfTourismAndPlaces")]
        [ProducesResponseType(typeof(TypeOfTourismAndPlacesResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTypeOfTourismandPlaces(string name, CancellationToken cancellationToken)
        {
            var result = await _typeOfTourismService.GetTypeOfTourismAndPlaces(name, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        [HttpGet("All-TypeOfTourismName")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTypeOfTourismName(CancellationToken cancellationToken)
        {
            var result = await _typeOfTourismService.AllTypeOfTourismName(cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        
    }
}
