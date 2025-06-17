using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tourism_Api.Entity.Admin;
using Tourism_Api.Entity.Places;
using Tourism_Api.Entity.Programs;
using Tourism_Api.Entity.Tourguid;
using Tourism_Api.Services.IServices;

namespace Tourism_Api.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = DefaultRoles.Admin, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
[Produces("application/json")]

public class AdminController(IAdminServices adminServices) : ControllerBase
{
    private readonly IAdminServices adminServices = adminServices;

    /// <summary>
    /// Add a new place to the database
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("AddPlace")]
    [ProducesResponseType( StatusCodes.Status200OK, Type = typeof(PlacesDetails))]
   
    public async Task<IActionResult> AddPlace(AddPlaceRequest request, CancellationToken cancellationToken)
    {
        var result = await adminServices.AddPlace(request, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpDelete("DeletePlace")]
    [ProducesResponseType( StatusCodes.Status200OK)]
    public async Task<IActionResult> DeletePlace(string PlaceName, CancellationToken cancellationToken)
    {
        var result = await adminServices.DeletePlace(PlaceName, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPut("UpdatePlace")]
    [ProducesResponseType( StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdatePlace(string PlaceName, UpdatePlaceRequest request, CancellationToken cancellationToken)
    {
        var result = await adminServices.UpdatePlace(PlaceName , request, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpGet("NotActiveTourguid")]
    [ProducesResponseType( StatusCodes.Status200OK, Type = typeof(AllNotActiveTourguid))]
    public async Task<IActionResult> NotActiveTourguid(CancellationToken cancellationToken)
    {
        var result = await adminServices.NotActiveTourguid(cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpDelete("DeleteTourguid")]
    [ProducesResponseType( StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteTourguid(string TourguidId, CancellationToken cancellationToken)
    {
        var result = await adminServices.DeleteTourguid(TourguidId, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    //[HttpPost("AddTourguidPlace")]
    //public async Task<IActionResult> AddTourguidPlace(AddTourguidPlaceRequest request, CancellationToken cancellationToken)
    //{
    //    var result = await adminServices.AddTourguidPlace(request.TourguidId , request.PlaceName, cancellationToken);
    //    return result.IsSuccess ? Ok() : result.ToProblem();
    //}
    [HttpDelete("DeleteTourguidPlace")]
    [ProducesResponseType( StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteTourguidPlace(AddTourguidPlaceRequest request, CancellationToken cancellationToken)
    {
        var result = await adminServices.DeleteTourguidPlace(request.TourguidId, request.PlaceName, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpGet("dashboard")]
    [ProducesResponseType( StatusCodes.Status200OK, Type = typeof(Dashboard))]
    public async Task<IActionResult> dashboard(CancellationToken cancellationToken)
    {
        var result = await adminServices.DisplayAll(cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("TransferRequest")]
    [ProducesResponseType( StatusCodes.Status200OK, Type = typeof(TransferRequests))]
    public async Task<IActionResult> TransferRequest(CancellationToken cancellationToken)
    {
        var result = await adminServices.TransferRequest(cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpDelete("TransferRequestDecline")]
    [ProducesResponseType( StatusCodes.Status200OK)]
    public async Task<IActionResult> TransferRequestDecline(string TourguidId, CancellationToken cancellationToken)
    {
        var result = await adminServices.TransferRequestDecline(TourguidId, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpGet("DisplayAllPrograms")]
    [ProducesResponseType( StatusCodes.Status200OK, Type = typeof(List<string>))]
    [AllowAnonymous]
    public async Task<IActionResult> DisplayAllPrograms(CancellationToken cancellationToken)
    {
        var result = await adminServices.DisplayAllPrograms(cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpPut("MoveTourguidAccapt")]
    [ProducesResponseType( StatusCodes.Status200OK)]
    public async Task<IActionResult> MoveTourguidAccapt(TourguidTrips request , CancellationToken cancellationToken)
    {

        var result = await adminServices.MoveTourguidAccapt(request, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPut("ActiveTourguid")]
    [ProducesResponseType( StatusCodes.Status200OK)]
    public async Task<IActionResult> ActiveTourguid(string TourguidId, CancellationToken cancellationToken)
    {
        var result = await adminServices.ActiveTourguid(TourguidId, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpGet("ContactUsProblems")]
    [ProducesResponseType( StatusCodes.Status200OK, Type = typeof(ContactUsProblemsResponse))]
    public async Task<IActionResult> GetAllContactUsProblems(CancellationToken cancellationToken)
    {
        var result = await adminServices.GetAllContactUsProblems(cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost("ReplyToContactUs")]
    [ProducesResponseType( StatusCodes.Status200OK)]
    public async Task<IActionResult> ReplyToContactUs([FromBody] AdminReplyContactUsRequest request, CancellationToken cancellationToken)
    {
        var result = await adminServices.ReplyToContactUs(request, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpDelete("DeleteContactUsProblem")]
    [ProducesResponseType( StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteContactUsProblem(int problemId, CancellationToken cancellationToken)
    {
        var result = await adminServices.DeleteContactUsProblem(problemId, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpGet("ResolvedContactUsProblems")]
    [ProducesResponseType( StatusCodes.Status200OK, Type = typeof(ContactUsProblemsResponse))]
    public async Task<IActionResult> GetAllResolvedContactUsProblems(CancellationToken cancellationToken)
    {
        var result = await adminServices.GetAllResolvedContactUsProblems(cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpPost("AddTrip")]
    [ProducesResponseType( StatusCodes.Status200OK)]
    public async Task<IActionResult> AddTrip(AddTripRequest request, CancellationToken cancellationToken)
    {
        var result = await adminServices.AddTrip(request, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPut("UpdateTrip")]
    [ProducesResponseType( StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateTrip(string tripName, UpdateTripRequest request, CancellationToken cancellationToken)
    {
        var result = await adminServices.UpdateTrip(tripName, request, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpDelete("DeleteTrip")]
    [ProducesResponseType( StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteTrip(string tripName, CancellationToken cancellationToken)
    {
        var result = await adminServices.DeleteTrip(tripName, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    [HttpDelete("DeleteComment")]
    [ProducesResponseType( StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteComment(int commentId, CancellationToken cancellationToken)
    {
        var result = await adminServices.DeleteAnyComment(commentId, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
}
