using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using BLL.Dtos.Auth;
using BLL.Dtos.Messages;
using BLL.Interfaces;
using BLL.Models.Auth;
using DAL.Entities;
using DAL.Entities.Auth;
using GeneralLibrary.Configuration;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BLL.Services;

public partial class AuthService : ServiceBasePg, IAuthService
{
    private readonly TokenService _tokenService;

    private HashOptions HashOptions { get; }

    public AuthService(IHttpContextAccessor accessor, TokenService tokenService, HashOptions hashOptions) :
        base(accessor)
    {
        _tokenService = tokenService;
        HashOptions = hashOptions;
    }


    public async Task RegisterUserByFirebase()
    {
        //TODO: check if username is not unique, and then change it with added numbers to end.
    }

    private async Task RegisterUser(UserAuthM user)
    {
        User dbUser = new()
        {
            Name = user.Name,
            StatsOneVsOne = new StatsOneVsOne(),
            StatsTwoVsTwo = new StatsTwoVsTwo()
        };
        foreach (var authInfoM in user.AuthInfos)
            switch (authInfoM)
            {
                case AuthInfoFirebaseM firebaseM:
                    dbUser.AuthInfos.Add(new AuthInfoFirebase() {FirebaseUuid = firebaseM.FirebaseUuid});
                    break;
                case AuthInfoMailM mailM:
                    dbUser.AuthInfos.Add(new AuthInfoMail() {Email = mailM.Email, HashPassword = mailM.HashPassword});
                    break;
            }

        Db.Users.Add(dbUser);
        await Db.SaveChangesAsync();
    }

    public UserClaimData? GetUserClaims()
    {
        // return new ()
        // {
        //     Id =   HttpContext.User.Id
        // }
        //

        var claims = HttpContext.User.Claims.ToList();
        var id = claims.FirstOrDefault(u => u.Type == ClaimTypes.Sid);
        var name = claims.FirstOrDefault(u => u.Type == ClaimTypes.Name);
        if (id == null || name == null) return null;
        var data = new UserClaimData()
        {
            Id = Guid.Parse(id.Value),
            Name = id.Value
        };
        return data;
    }

    public async Task<MessageBaseDto> Logout(LogoutDto dto)
    {
        var claims = HttpContext.User.Claims.ToList();
        var id = claims.FirstOrDefault(u => u.Type == ClaimTypes.Sid);
        var user = await Db.Users.Include(u => u.AuthInfos).FirstOrDefaultAsync(u => u.Id == Guid.Parse(id.Value));
        var authInfo = user.AuthInfos.FirstOrDefault(a => a is AuthInfoMail) as AuthInfoMail;
        authInfo.RefreshTokens = authInfo.RefreshTokens.Where(t => t != dto.RefreshToken).ToArray();
        await Db.SaveChangesAsync();
        return new MessageBaseDto() {Message = "You hav been logouted", Success = true};
    }
}