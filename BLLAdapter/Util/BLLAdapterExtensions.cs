using DAL.Util;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BLLAdapter.Util;

public static class BLLAdapterExtensions
{
    public static void BllAdapterRegister(this IServiceCollection services, IConfiguration configuration)
    {
        services.DalRegister(configuration);
    }
}