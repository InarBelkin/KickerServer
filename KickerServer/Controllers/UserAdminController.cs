using BLL.Dtos.Auth;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KickerServer.Controllers.Admin;

[ApiController]
[Route("api/[controller]")]
public sealed class UserAdminController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<UserAdminController> _logger;


    public UserAdminController(IAuthService authService, ILogger<UserAdminController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult> CreateUser(RegisterByEmailDto regDto)
    {
        await _authService.RegisterUserByEmail(regDto);
        _logger.LogInformation("user registered");
        return Ok();
    }
}