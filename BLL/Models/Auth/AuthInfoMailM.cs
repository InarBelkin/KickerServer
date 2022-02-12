using static System.String;

namespace BLL.Models.Auth;

public class AuthInfoMailM : IAuthInfoM
{
    public string Email { get; set; } = Empty;
    public string HashPassword { get; set; } = Empty;
}