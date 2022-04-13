using System.ComponentModel.DataAnnotations;
using static System.String;

namespace BLL.Dtos.Auth;

public class LoginDto
{
    [Required(ErrorMessage = "Email is required")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = Empty;

    public string Password { get; set; } = Empty;
}