using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BLL.Dtos.Auth;
using BLL.Dtos.Messages;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace KickerServer.Controllers.Admin;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IAuthService _authService;

    public UsersController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("registration/mail")]
    public async Task<ActionResult<RegisterAnswerDto>> Register(RegisterByEmailDto registerDto)
    {
        var answerDto = await _authService.RegisterUserByEmail(registerDto);
        return Ok(answerDto);
    }


    [HttpPost("login")]
    public async Task<ActionResult<LoginAnswerDto>> Login(LoginDto loginDto)
    {
        var valid = Validator.TryValidateObject(loginDto, new ValidationContext(loginDto), new List<ValidationResult>(),
            true);
        var answerDto = await _authService.LoginByEmail(loginDto);
        return Ok(answerDto);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<LoginAnswerDto>> Refresh(LoginRefreshDto refreshDto)
    {
        var answer = await _authService.LoginByEmailRefresh(refreshDto);

        return Ok(answer);
    }

    [HttpPost("logout")]
    public async Task<ActionResult<MessageBaseDto>> Logout(LogoutDto logoutDto)
    {
        return Ok(await _authService.Logout(logoutDto));
    }
}