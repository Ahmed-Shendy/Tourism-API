using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tourism_Api.Entity.Comment;
using Tourism_Api.Entity.Places;
using Tourism_Api.Services.IServices;

namespace Tourism_Api.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UserController (IUserServices userServices) : ControllerBase
{
    private readonly IUserServices userServices = userServices;

    
    [HttpPost("AddComment")]
    public async Task<IActionResult> AddComment(AddComment request , CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracts ID
        var result = await userServices.AddComment(userId! ,request , cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpDelete("DeleteComment")]
    public async Task<IActionResult> DeleteComment(int CommentId, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracts ID
        var result = await userServices.DeleteComment(userId!, CommentId, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPut("UpdateComment")]
    public async Task<IActionResult> UpdateComment(UpdateComment request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracts ID
        var result = await userServices.UpdateComment(userId!, request, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    [HttpPost("Addrate")]
    public async Task<IActionResult> Addrate(AddRate request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracts ID
        var result = await userServices.Addrate(userId!, request, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }


}
