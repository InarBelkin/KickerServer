namespace BLLAdapter.Models.Auth;

public class UserAuthM
{
    public string Name { get; set; } = string.Empty;
    public List<IAuthInfoM> AuthInfos = new List<IAuthInfoM>();
}