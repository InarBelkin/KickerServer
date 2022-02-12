using BLL.Dtos.Auth;

namespace BLL.Interfaces;

public interface IAuthService
{
    public Task RegisterUserByEmail(RegisterByEmailDto regDto);
}