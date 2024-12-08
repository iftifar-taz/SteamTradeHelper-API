using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using SteamTradeHelper.Mappings.Profiles.Bots;
using SteamTradeHelper.Mappings.Profiles.Cards;
using SteamTradeHelper.Mappings.Profiles.Games;

namespace SteamTradeHelper.Mappings.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCustomAutoMapper(this IServiceCollection services)
        {
            services.AddSingleton<Profile>(provider => new GamesClientToDbProfile());
            services.AddSingleton<Profile>(provider => new GamesDbToDtoProfile());

            services.AddSingleton<Profile>(provider => new CardsClientToDbProfile());
            services.AddSingleton<Profile>(provider => new CardsDbToDtoProfile());

            services.AddSingleton<Profile>(provider => new BotsAsfClientToDbProfile());
            services.AddSingleton<Profile>(provider => new BotsDbToDtoProfile());

            services.AddSingleton(provider => CreateMapperConfiguration(provider).CreateMapper());
        }

        private static MapperConfiguration CreateMapperConfiguration(IServiceProvider provider)
        {
            return new MapperConfiguration(c => c.AddProfiles(provider.GetServices<Profile>()));
        }
    }
}
