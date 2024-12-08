using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
