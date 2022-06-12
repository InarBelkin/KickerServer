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

        var environmentName = Environment.GetEnvironmentVariable("ConnectionStrings:RedisConnection2");
        var envname2 = configuration.GetSection("ConnectionStrings").GetChildren().ToList();
        if (envname2.Count != 0)
            Console.WriteLine(envname2);

        var env3 = configuration["ConnectionStrings"];
        if(env3!= null) Console.WriteLine();
        
        Console.WriteLine(environmentName);


        var redisConnectionString = configuration.GetConnectionString("RedisConnection");
        var multiplexer = ConnectionMultiplexer.Connect(redisConnectionString, c => { c.AbortOnConnectFail = false; });

        services.AddSingleton<ConnectionMultiplexer>(multiplexer);

        services.AddScoped<ILobbyRepository, LobbyRepository>();
        services.AddScoped<IConnectedUsersRepository, ConnectedUsersRepository>();
    }
}