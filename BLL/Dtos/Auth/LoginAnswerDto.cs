using static System.String;

namespace BLL.Dtos.Auth;

public class LoginAnswerDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = Empty;
    public string Token { get; set; } = Empty;
}