using System.Dynamic;
using System.Text;
using BLL.Interfaces;
using BLL.Services;
using DAL.Util;
using GeneralLibrary.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace BLL.Util;

public static class BLLExtensions
{
    public static void BLLRegister(this IServiceCollection collection, IConfiguration configuration)
    {
        collection.AddScoped<IAuthService, AuthService>();
        collection.AddScoped<IStatsService, StatsService>();
        collection.AddScoped<ILobbyService, LobbyService>();
        collection.AddScoped<ILobbyMessagesService, LobbyMessagesService>();
        collection.AddScoped<ILobbyUtilService, LobbyUtilService>();
        collection.AddScoped<IBattleService, BattleService>();

        collection.AddScoped<TokenService>();

        collection.AddAutoMapper(typeof(MapperProfile));
        collection.AuthRegister(configuration);

        collection.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

        collection.DalRegister(configuration);
    }

    private static void AuthRegister(this IServiceCollection collection, IConfiguration configuration)
    {
        var options = configuration.GetSection("JwtOptions").Get<JwtOptions>()!;


        collection.AddSingleton(options);

        var hashOptions = configuration.GetSection("HashOptions").Get<HashOptions>();
        collection.AddSingleton(hashOptions);


        collection.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
        {
            opt.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = options.ValidateIssuer,
                ValidIssuer = options.Issuer,
                ValidateAudience = options.ValidateAudience,
                ValidAudience = options.Audience,
                ValidateLifetime = options.ValidateLifetime,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Key))
            };
        });
    }
}