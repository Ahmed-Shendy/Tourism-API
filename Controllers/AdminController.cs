using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tourism_Api.Entity.Places;
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
    public async Task<IActionResult> DeletePlace(string PlaceId, CancellationToken cancellationToken)
    {
        var result = await adminServices.DeletePlace(PlaceId, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPut("UpdatePlace")]
    public async Task<IActionResult> UpdatePlace(AddPlaceRequest request, CancellationToken cancellationToken)
    {
        var result = await adminServices.UpdatePlace(request, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPost("AddTourguid")]
    public async Task<IActionResult> AddTourguid(AddTourguidRequest request, CancellationToken cancellationToken)
    {
        var result = await adminServices.AddTourguid(request, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpDelete("DeleteTourguid")]
    public async Task<IActionResult> DeleteTourguid(string TourguidId, CancellationToken cancellationToken)
    {
        var result = await adminServices.DeleteTourguid(TourguidId, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPost("AddTourguidPlace")]
    public async Task<IActionResult> AddTourguidPlace(AddTourguidPlaceRequest request, CancellationToken cancellationToken)
    {
        var result = await adminServices.AddTourguidPlace(request.TourguidId , request.PlaceName, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpDelete("DeleteTourguidPlace")]
    public async Task<IActionResult> DeleteTourguidPlace(AddTourguidPlaceRequest request, CancellationToken cancellationToken)
    {
        var result = await adminServices.DeleteTourguidPlace(request.TourguidId, request.PlaceName, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpGet("DisplayAllTourguid")]
    public async Task<IActionResult> DisplayAllTourguid(CancellationToken cancellationToken)
    {
        var result = await adminServices.DisplayAllTourguid(cancellationToken);
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

}
