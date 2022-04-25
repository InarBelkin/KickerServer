using static System.String;

namespace BLL.Models.Auth;

public class UserClaimData
{
    public Guid Id { get; set; }
    public string Name { get; set; } = Empty;
}