using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Tourism_Api.Services.IServices;

namespace Tourism_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [EnableRateLimiting(RateLimiters.Concurrency)]
    public class TypeOfTourismController(ITypeOfTourismService typeOfTourismService) : ControllerBase
    {
        private readonly ITypeOfTourismService _typeOfTourismService = typeOfTourismService;
        [HttpGet("All-TypeOfTourism")]
        public async Task<IActionResult> GetAllTypeOfTourism(CancellationToken cancellationToken)
        {
            var result = await _typeOfTourismService.GetAllTypeOfTourismAsync(cancellationToken);
            return Ok(result.Value);
        }
        [HttpGet("TypeOfTourismAndPlaces")]
        public async Task<IActionResult> GetTypeOfTourismAndPlaces(String Name, CancellationToken cancellationToken)
        {
            var result = await _typeOfTourismService.GetTypeOfTourismAndPlacesAsync(Name, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        [HttpGet("All-TypeOfTourismName")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTypeOfTourismName(CancellationToken cancellationToken)
        {
            var result = await _typeOfTourismService.AllTypeOfTourismName(cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
    }
}
