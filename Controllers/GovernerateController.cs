using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tourism_Api.Services.IServices;

namespace Tourism_Api.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GovernerateController(IGovernorateService governorateService) : ControllerBase
    {
        private readonly IGovernorateService _governorateService = governorateService;
        [HttpGet]
        public async Task<IActionResult> GetGovernorate(CancellationToken cancellationToken)
        {
            var result = await _governorateService.GetGovernorate(cancellationToken);
            return Ok(result.Value);
        }
        [HttpGet()]
        public async Task<IActionResult> GetGovernorateAndPlaces(String Name, CancellationToken cancellationToken)
        {
            var result = await _governorateService.GetGovernorateAndPlacesAsync(Name, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
    }
}
