namespace DAL.Entities.Auth;

public class AuthInfoMail : AuthInfo
{
    public string Email { get; set; } = string.Empty;
    public string HashPassword { get; set; } = string.Empty;

    public string[] RefreshTokens { get; set; } = Array.Empty<string>();
}