using Microsoft.Extensions.DependencyInjection;
using SteamTradeHelper.Context.Models;
using SteamTradeHelper.Repositories.Contracts;

namespace SteamTradeHelper.Repositories.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCustomRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBaseRepository<Game>, BaseRepository<Game>>();
            services.AddScoped<IBaseRepository<Card>, BaseRepository<Card>>();
            services.AddScoped<IBaseRepository<Bot>, BaseRepository<Bot>>();
        }
    }
}
