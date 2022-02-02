using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KickerServer.Controllers.Admin;

[ApiController]
[Route("api/[controller]")]
public sealed class UserAdminController : ControllerBase
{
    private readonly IAuthService _authService;

    public UserAdminController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    public async Task<ActionResult> CreateUser( string email)
    {
        await _authService.RegisterUserByEmail(email);
        return Ok();
    }
}