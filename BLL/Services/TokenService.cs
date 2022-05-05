using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DAL.Entities;
using DAL.Entities.Auth;
using GeneralLibrary.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BLL.Services;

public class TokenService : ServiceBasePg
{
    private readonly JwtOptions _jwtOptions;

    public TokenService(IHttpContextAccessor accessor, JwtOptions jwtOptions) : base(accessor)
    {
        _jwtOptions = jwtOptions;
    }

    public string AccessToken(IEnumerable<Claim> claims)
    {
        var jwt = new JwtSecurityToken(issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience, claims: claims, expires: DateTime.UtcNow.Add(TimeSpan.FromDays(7)),
            signingCredentials: new SigningCredentials(_jwtOptions.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    public string RefreshToken(IEnumerable<Claim> claims)
    {
        var jwt = new JwtSecurityToken(issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience, claims: claims, expires: DateTime.UtcNow.AddMonths(1),
            signingCredentials: new SigningCredentials(_jwtOptions.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    public Guid? GetUserIdByToken(string refreshToken)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key)),
            ValidateLifetime = true //here we are saying that we don't care about the token's expiration date
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;
        var prinicpal = tokenHandler.ValidateToken(refreshToken, tokenValidationParameters, out securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        var id = prinicpal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
        return id != null ? Guid.Parse(id!) : null;
    }

    public string[] NewTokens(string[] oldTokens,string oldToken, string newToken)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key)),
            ValidateLifetime = true //here we are saying that we don't care about the token's expiration date
        };

        var ret = new List<string>();


        foreach (var token in oldTokens)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var prinicpal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            if (securityToken != null)
            {
                if (token != oldToken)
                {
                    ret.Add(token);
                }
            }
        }
        
        ret.Add(newToken);

        return ret.ToArray();
    }
}