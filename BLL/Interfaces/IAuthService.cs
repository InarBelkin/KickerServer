using BLL.Dtos.Auth;

namespace BLL.Interfaces;

public interface IAuthService
{
    public Task<RegisterAnswerDto> RegisterUserByEmail(RegisterByEmailDto regDto);
    public Task<LoginAnswerDto> LoginByEmail(LoginDto loginDto);
}