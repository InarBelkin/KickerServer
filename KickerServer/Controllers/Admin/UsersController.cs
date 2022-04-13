using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BLL.Dtos.Auth;
using BLL.Interfaces;
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
        if (answerDto.Success)
            return Ok(answerDto);
        else return BadRequest(answerDto);
    }


    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginDto loginDto)
    {
        var valid = Validator.TryValidateObject(loginDto, new ValidationContext(loginDto),new List<ValidationResult>(),true);
        var answerDto = await _authService.LoginByEmail(loginDto);

        if (answerDto.Success)
            return Ok(answerDto);
        else return BadRequest(answerDto);
    }
}