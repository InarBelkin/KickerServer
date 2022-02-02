using BLL.Interfaces;
using BLLAdapter.Interfaces;
using BLLAdapter.Models.Auth;
using Microsoft.AspNetCore.Http;

namespace BLL.Services;

public class AuthService : ServiceBase, IAuthService
{
    private readonly IAuthRepos _authRepos;
    
    public AuthService(IAuthRepos authRepos,IHttpContextAccessor accessor) : base(accessor)
    {
        _authRepos = authRepos;
    }

    public async Task RegisterUserByEmail(string email)
    {
        await _authRepos.AddNewUser(new UserAuthM()
            {Name = email, AuthInfos = new() {new AuthInfoMailM() {Email = email, HashPassword = "pip"}}});
    }


}