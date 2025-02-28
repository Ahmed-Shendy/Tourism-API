using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tourism_Api.Entity.user;
using Tourism_Api.Repository;

namespace Tourism_Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class authenticatController(
    Authenticat authenticat
    
    
    ): ControllerBase
{
    private readonly Authenticat authenticat = authenticat;

    [HttpPost("Register")]
    public async Task<IActionResult> RegisterAsync(UserRequest request, CancellationToken cancellationToken = default)
    {
        var result = await authenticat.RegisterAsync(request, cancellationToken);
        if (result == null)
            return BadRequest();
        else if (result.Name == "Email must unique")
            return Conflict("Email must unique");
        return Ok(result);
    }
    [HttpPost("Login")]
    public async Task<IActionResult> LoginAsync(userLogin request, CancellationToken cancellationToken = default)
    {
        var result = await authenticat.LoginAsync(request, cancellationToken);
        if (result == null)
            return NotFound();
        else if (result.Name == "Looked user For 5 Minutes")
            return Conflict("Looked user For 5 Minutes");
        return Ok(result);
    }
    [HttpPost("GetRefreshToken")]
    public async Task<IActionResult> GetRefreshTokenAsync(UserRefreshToken request, CancellationToken cancellationToken = default)
    {
        var result = await authenticat.GetRefreshToken(request, cancellationToken);
        if (result == null)
            return BadRequest();
        return Ok(result);
    }
}
