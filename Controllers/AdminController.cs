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
public class AdminController(IAdminServices adminServices) : ControllerBase
{
    private readonly IAdminServices adminServices = adminServices;

    [HttpPost("AddPlace")]
    public async Task<IActionResult> AddPlace(AddPlaceRequest request, CancellationToken cancellationToken)
    {
        var result = await adminServices.AddPlace(request, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpDelete("DeletePlace")]
    public async Task<IActionResult> DeletePlace(string PlaceName, CancellationToken cancellationToken)
    {
        var result = await adminServices.DeletePlace(PlaceName, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPut("UpdatePlace")]
    public async Task<IActionResult> UpdatePlace(string PlaceName, UpdatePlaceRequest request, CancellationToken cancellationToken)
    {
        var result = await adminServices.UpdatePlace(PlaceName , request, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpGet("NotActiveTourguid")]
    public async Task<IActionResult> NotActiveTourguid(CancellationToken cancellationToken)
    {
        var result = await adminServices.NotActiveTourguid(cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpDelete("DeleteTourguid")]
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
    public async Task<IActionResult> DeleteTourguidPlace(AddTourguidPlaceRequest request, CancellationToken cancellationToken)
    {
        var result = await adminServices.DeleteTourguidPlace(request.TourguidId, request.PlaceName, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpGet("dashboard")]
    public async Task<IActionResult> dashboard(CancellationToken cancellationToken)
    {
        var result = await adminServices.DisplayAll(cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("TransferRequest")]
    public async Task<IActionResult> TransferRequest(CancellationToken cancellationToken)
    {
        var result = await adminServices.TransferRequest(cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpDelete("TransferRequestDecline")]
    public async Task<IActionResult> TransferRequestDecline(string TourguidId, CancellationToken cancellationToken)
    {
        var result = await adminServices.TransferRequestDecline(TourguidId, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpGet("DisplayAllPrograms")]
   // [AllowAnonymous]
    public async Task<IActionResult> DisplayAllPrograms(CancellationToken cancellationToken)
    {
        var result = await adminServices.DisplayAllPrograms(cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpPut("MoveTourguidAccapt")]
    public async Task<IActionResult> MoveTourguidAccapt(TourguidTrips request , CancellationToken cancellationToken)
    {

        var result = await adminServices.MoveTourguidAccapt(request, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPut("ActiveTourguid")]
    public async Task<IActionResult> ActiveTourguid(string TourguidId, CancellationToken cancellationToken)
    {
        var result = await adminServices.ActiveTourguid(TourguidId, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpGet("ContactUsProblems")]
    public async Task<IActionResult> GetAllContactUsProblems(CancellationToken cancellationToken)
    {
        var result = await adminServices.GetAllContactUsProblems(cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost("ReplyToContactUs")]
    public async Task<IActionResult> ReplyToContactUs([FromBody] AdminReplyContactUsRequest request, CancellationToken cancellationToken)
    {
        var result = await adminServices.ReplyToContactUs(request, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpDelete("DeleteContactUsProblem")]
    public async Task<IActionResult> DeleteContactUsProblem(int problemId, CancellationToken cancellationToken)
    {
        var result = await adminServices.DeleteContactUsProblem(problemId, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpGet("ResolvedContactUsProblems")]
    public async Task<IActionResult> GetAllResolvedContactUsProblems(CancellationToken cancellationToken)
    {
        var result = await adminServices.GetAllResolvedContactUsProblems(cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
