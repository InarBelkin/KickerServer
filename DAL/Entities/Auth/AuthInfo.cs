using System.ComponentModel.DataAnnotations;

namespace DAL.Entities.Auth;

public class AuthInfo : BaseEntity
{
    [Required] public User? User { get; set; }
}