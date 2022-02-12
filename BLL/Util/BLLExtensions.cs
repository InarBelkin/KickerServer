using BLL.Interfaces;
using BLL.Services;
using BLLAdapter.Util;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Util;

public static class BLLExtensions
{
    public static void BLLRegister(this IServiceCollection collection, IConfiguration configuration)
    {
        collection.AddScoped<IAuthService, AuthService>();
        collection.BllAdapterRegister(configuration);
    }
}