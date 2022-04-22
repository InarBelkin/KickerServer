using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BLL.Dtos.Auth;
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
}