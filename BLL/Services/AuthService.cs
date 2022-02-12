using BLL.Dtos.Auth;
using BLL.Interfaces;
using BLL.Models.Auth;
using DAL.Entities;
using DAL.Entities.Auth;
using Microsoft.AspNetCore.Http;

namespace BLL.Services;

public class AuthService : ServiceBasePg, IAuthService
{
    public AuthService(IHttpContextAccessor accessor) : base(accessor)
    {
    }

    public async Task RegisterUserByEmail(RegisterByEmailDto regDto)
    {
        await RegisterUser(new UserAuthM()
        {
            Name = regDto.Name, AuthInfos = {new AuthInfoMailM() {Email = regDto.Email, HashPassword = regDto.Password}}
        });
    }

    public async Task
        RegisterUserByFirebase() //TODO: check if username is not unique, and then change it with added numbers to end.
    {
    }

    internal async Task RegisterUser(UserAuthM user)
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