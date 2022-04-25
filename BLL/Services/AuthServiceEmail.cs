using System.Security.Claims;
using System.Text;
using BLL.Dtos.Auth;
using BLL.Interfaces;
using BLL.Models.Auth;
using DAL.Entities;
using DAL.Entities.Auth;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BLL.Services;

public partial class AuthService : IAuthService
{
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
                a.Email == loginDto.Email.Trim(' ').ToLower() && a.HashPassword == HashPass(loginDto.Password))
            .Include(a => a.User)
            .FirstOrDefaultAsync();

        if (authInfoMail?.User == null)
            return new LoginAnswerDto() {Success = false, Message = "User doesn't not exists or password is incorrect"};

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, authInfoMail.User.Name),
            new Claim(ClaimTypes.Sid, authInfoMail.User.Id.ToString()),
        };

        var accessToken = _tokenService.AccessToken(claims);
        var refreshToken = _tokenService.RefreshToken(claims);

        authInfoMail.RefreshTokens = authInfoMail.RefreshTokens.Concat(new[] {refreshToken}).ToArray();
        Db.Entry(authInfoMail).State = EntityState.Modified;
        await Db.SaveChangesAsync();

        return new LoginAnswerDto()
        {
            Success = true,
            Message = $"You have successfully logged as {authInfoMail.User?.Name}",
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public async Task<LoginAnswerDto> LoginByEmailRefresh(LoginRefreshDto dto)
    {
        var userId = _tokenService.GetUserIdByToken(dto.RefreshToken);

        if (userId == null)
            return new LoginAnswerDto()
            {
                Success = false,
                Message = "Token was expired or incorrect"
            };

        var user = await Db.Users.Where(u => u.Id == userId).Include(u => u.AuthInfos).FirstOrDefaultAsync();
        var authInfo = user.AuthInfos.FirstOrDefault(u => u is AuthInfoMail) as AuthInfoMail;


        if (authInfo.RefreshTokens.Count(t => t == dto.RefreshToken) > 0)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
            };

            var accessToken = _tokenService.AccessToken(claims);
            var refreshToken = _tokenService.RefreshToken(claims);
            authInfo!.RefreshTokens = _tokenService.NewTokens(authInfo.RefreshTokens, dto.RefreshToken, refreshToken);
            Db.Entry(authInfo).State = EntityState.Modified;
            await Db.SaveChangesAsync();
            
            return new LoginAnswerDto()
            {
                Success = true,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Message = $"You have successfully logged as {user?.Name}"
            };
        }

        return new LoginAnswerDto()
        {
            Success = false,
            Message = "Refresh token does not exist"
        };
    }

    public async Task LogoutEverywhere(Guid userId)
    {
        var user = await Db.Users.Where(u => u.Id == userId).Include(u => u.AuthInfos).FirstOrDefaultAsync();
        var authInfo = user.AuthInfos.FirstOrDefault(u => u is AuthInfoMail) as AuthInfoMail;
        if (authInfo != null) authInfo.RefreshTokens = Array.Empty<string>();
    }
}