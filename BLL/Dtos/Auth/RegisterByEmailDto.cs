using static System.String;

namespace BLL.Dtos.Auth;

public class RegisterByEmailDto
{
    public string Name { get; set; } = Empty;
    public string Email { get; set; } = Empty;
    public string Password { get; set; } = Empty;
}