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
