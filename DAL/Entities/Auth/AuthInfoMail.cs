namespace DAL.Entities.Auth;

public class AuthInfoMail : AuthInfo
{
    public string Email { get; set; } = String.Empty;
    public string HashPassword { get; set; } =String.Empty;
}