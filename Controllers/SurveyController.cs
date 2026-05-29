using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tourism_Api.Entity.Survey;
using Tourism_Api.Services.IServices;

namespace Tourism_Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
[Produces("application/json")]
public class SurveyController(ISurveyService surveyService) : ControllerBase
{
    private readonly ISurveyService surveyService = surveyService;

    [HttpPost("AddSurvey")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> AddSurvey([FromBody] AddSurveyRequest request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var result = await surveyService.AddSurvey(userId!, request, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    [HttpGet("MySurveys")]
    [ProducesResponseType(typeof(List<SurveyResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMySurveys(CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var result = await surveyService.GetMySurveys(userId!, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet("AllSurveys")]
    [Authorize(Roles = DefaultRoles.Admin, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(List<SurveyResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllSurveys(CancellationToken cancellationToken)
    {
        var result = await surveyService.GetAllSurveys(cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpDelete("DeleteSurvey")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteSurvey(int surveyId, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var isAdmin = User.IsInRole(DefaultRoles.Admin);

        var result = await surveyService.DeleteSurvey(surveyId, userId!, isAdmin, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
}
