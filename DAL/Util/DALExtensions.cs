using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace DAL.Util;

public static class DALExtensions
{
    public static void DalRegister(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<KickerContext>(options => options.UseNpgsql(connectionString));

        var redisConnectionString = configuration.GetConnectionString("RedisConnection");
        var multiplexer = ConnectionMultiplexer.Connect(redisConnectionString);
        services.AddSingleton<ConnectionMultiplexer>(multiplexer);

        services.AddScoped<ILobbyRepository, LobbyRepository>();
    }
}