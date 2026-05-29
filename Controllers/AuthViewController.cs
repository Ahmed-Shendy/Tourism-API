using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tourism_Api.Services.IServices;

namespace Tourism_Api.Controllers;

[AllowAnonymous]
[ApiExplorerSettings(IgnoreApi = true)]
[Route("auth")]
public class AuthViewController(IAuthenticatServices authenticatServices) : Controller
{
    private readonly IAuthenticatServices authenticatServices = authenticatServices;

    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmailAsync([FromQuery] string userId, [FromQuery] string token, CancellationToken cancellationToken)
    {
        var model = await authenticatServices.ConfirmEmailAsync(userId, token, cancellationToken);

        return View("~/Views/Auth/ConfirmEmailResult.cshtml", model);
    }
}
