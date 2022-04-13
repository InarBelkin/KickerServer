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

public class AuthService : ServiceBasePg, IAuthService
{
    private JwtOptions JwtOptions { get; }
    private HashOptions HashOptions { get; }

    public AuthService(IHttpContextAccessor accessor, JwtOptions jwtOptions, HashOptions hashOptions) : base(accessor)
    {
        JwtOptions = jwtOptions;
        HashOptions = hashOptions;
    }


    public async Task<RegisterAnswerDto> RegisterUserByEmail(RegisterByEmailDto regDto)
    {
        if (await Db.Users.CountAsync(u => u.Name == regDto.Name) != 0)
            return new()
            {
                Success = false,
                Message = "This username already exist"
            };
        if (await Db.AuthInfosMail.CountAsync(a => a.Email == regDto.Email) != 0)
            return new()
            {
                Success = false,
                Message = "this email already exist",
            };

        await RegisterUser(new UserAuthM()
        {
            Name = regDto.Name,
            AuthInfos =
            {
                new AuthInfoMailM() {Email = regDto.Email.ToLower().Trim(' '), HashPassword = HashPass(regDto.Password)}
            }
        });

        return new()
        {
            Success = true,
            Message = $"You have successfully registered as {regDto.Name} \n with email {regDto.Email}"
        };
    }

    private string HashPass(string password) =>
        Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: HashOptions.Salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8
        ));

    public async Task<LoginAnswerDto> LoginByEmail(LoginDto loginDto)
    {
        var authInfoMail = await Db.AuthInfosMail.Where(a =>
                a.Email == loginDto.Email && a.HashPassword == HashPass(loginDto.Password)).Include(a => a.User)
            .FirstOrDefaultAsync();

        if (authInfoMail?.User == null)
            return new LoginAnswerDto() {Success = false, Message = "User not existed or password is incorrect"};

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, authInfoMail.User.Name),
            new Claim(ClaimTypes.Sid, authInfoMail.User.Id.ToString())
        };

        var jwt = new JwtSecurityToken(issuer: JwtOptions.Issuer,
            audience: JwtOptions.Audience, claims: claims, expires: DateTime.UtcNow.Add(TimeSpan.FromDays(2)),
            signingCredentials: new SigningCredentials(JwtOptions.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256)
        );


        return new LoginAnswerDto()
        {
            Success = true,
            Message = $"You have successfully logged as {authInfoMail.User?.Name}",
            Token = new JwtSecurityTokenHandler().WriteToken(jwt)
        };
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