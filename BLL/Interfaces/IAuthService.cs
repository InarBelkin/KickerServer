namespace BLL.Interfaces;

public interface IAuthService
{
    public Task RegisterUserByEmail(string email);
}