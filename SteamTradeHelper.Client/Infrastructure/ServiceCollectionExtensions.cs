using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SteamTradeHelper.Client.Contracts;

namespace SteamTradeHelper.Client.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCustomClient(this IServiceCollection services)
        {
            services.AddScoped<ISteamClient, SteamClient>();
        }
    }
}
