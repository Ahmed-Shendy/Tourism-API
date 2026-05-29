using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Tourism_Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
[Produces("application/json")]
public class AhmedController : ControllerBase
{
    /// <summary>
    /// Ahmed controller endpoint
    /// </summary>
    /// <returns></returns>
    [HttpGet("GetData")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetData()
    {
        return Ok(new { message = "Ahmed Controller" });
    }
}
