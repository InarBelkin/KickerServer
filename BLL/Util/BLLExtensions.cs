using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLLAdapter.Util;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Util
{
    public static class BLLExtensions
    {
        public static void BLLRegister(this IServiceCollection collection, IConfiguration configuration)
        {
            collection.BllAdapterRegister(configuration);
        }
    }
}