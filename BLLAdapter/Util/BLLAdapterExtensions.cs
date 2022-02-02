using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Util;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BLLAdapter.Util
{
    public static class BLLAdapterExtensions
    {
        public static void BllAdapterRegister(this IServiceCollection services, IConfiguration configuration)
        {
            services.DalRegister(configuration);
        }
    }
}
