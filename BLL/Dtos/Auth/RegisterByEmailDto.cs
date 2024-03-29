using System.ComponentModel.DataAnnotations;
using static System.String;

namespace BLL.Dtos.Auth;

public class RegisterByEmailDto
{
    [Required] public string Name { get; set; } = Empty;

    [Required(ErrorMessage = "Email is required")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = Empty;

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = Empty;
}