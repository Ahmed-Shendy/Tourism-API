using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tourism_Api.Services.IServices;

namespace Tourism_Api.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TypeOfTourismController(ITypeOfTourismService typeOfTourismService) : ControllerBase
    {
        private readonly ITypeOfTourismService _typeOfTourismService = typeOfTourismService;
        [HttpGet]
        public async Task<IActionResult> GetAllTypeOfTourism(CancellationToken cancellationToken)
        {
            var result = await _typeOfTourismService.GetAllTypeOfTourismAsync(cancellationToken);
            return Ok(result.Value);
        }
        [HttpGet]
        public async Task<IActionResult> GetTypeOfTourismAndPlaces(String Name, CancellationToken cancellationToken)
        {
            var result = await _typeOfTourismService.GetTypeOfTourismAndPlacesAsync(Name, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
    }
}
