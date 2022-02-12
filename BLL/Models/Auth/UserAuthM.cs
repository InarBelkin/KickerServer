namespace BLL.Models.Auth;

public class UserAuthM
{
    public string Name { get; set; } = string.Empty;
    public List<IAuthInfoM> AuthInfos = new();
}