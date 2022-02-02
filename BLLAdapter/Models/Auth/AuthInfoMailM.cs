using static System.String;

namespace BLLAdapter.Models.Auth;

public class AuthInfoMailM : IAuthInfoM
{
    public string Email { get; set; } = Empty;
    public string HashPassword { get; set; } = Empty;
}