using BLL.Dtos.Auth;

namespace BLL.Interfaces;

public interface IAuthService
{
    public Task<RegisterAnswerDto> RegisterUserByEmail(RegisterByEmailDto regDto);
    public Task<LoginAnswerDto> LoginByEmail(LoginDto loginDto);
    public Task<LoginAnswerDto> LoginByEmailRefresh(LoginRefreshDto dto);
    public Task LogoutEverywhere(Guid userId);
}