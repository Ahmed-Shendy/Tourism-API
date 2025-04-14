using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tourism_Api.Services.IServices;

namespace Tourism_Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class MLValuesController(IProgramesServices programesServices) : ControllerBase
{
    private readonly IProgramesServices programesServices = programesServices;

    [HttpPost("ProgramName")]
    public async Task<IActionResult> ProgramName( string userid , string program, CancellationToken cancellationToken)
    {
        var result = await programesServices.GetProgram(userid , program , cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    [HttpGet("Tourism_Type")]
    public async Task<IActionResult> Tourism_Type(CancellationToken cancellationToken)
    {
        var result = await programesServices.Tourism_Type(cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("With_Family")]
    public async Task<IActionResult> With_Family(CancellationToken cancellationToken)
    {
        var result = await programesServices.With_Family(cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("Accommodation_Type")]
    public async Task<IActionResult> Accommodation_Type(CancellationToken cancellationToken)
    {
        var result = await programesServices.Accommodation_Type(cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("Preferred_Destination")]
    public async Task<IActionResult> Preferred_Destination(CancellationToken cancellationToken)
    {
        var result = await programesServices.Preferred_Destination(cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("Travel_Purpose")]
    public async Task<IActionResult> Travel_Purpose(CancellationToken cancellationToken)
    {
        var result = await programesServices.Travel_Purpose(cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

}
