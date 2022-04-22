using static System.String;

namespace BLL.Dtos.Auth;

public class LoginRefreshDto
{
    public string RefreshToken { get; set; } = Empty;
}