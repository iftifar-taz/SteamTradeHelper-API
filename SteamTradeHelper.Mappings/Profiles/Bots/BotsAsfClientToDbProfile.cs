using AutoMapper;
using SteamTradeHelper.Client.Models.Bots;
using SteamTradeHelper.Context.Models;

namespace SteamTradeHelper.Mappings.Profiles.Bots
{
    public class BotsAsfClientToDbProfile : Profile
    {
        public BotsAsfClientToDbProfile()
        {
            CreateMap<AsfBotResponse, Bot>();
        }
    }
}
