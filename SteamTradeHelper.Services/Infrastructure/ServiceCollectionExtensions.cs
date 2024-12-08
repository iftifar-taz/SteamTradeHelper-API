using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SteamTradeHelper.Services.Contracts;

namespace SteamTradeHelper.Services.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<ISteamSyncService, SteamSyncService>();
            services.AddScoped<IBotService, BotService>();
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<ICardService, CardService>();
        }
    }
}
