using AutoMapper;
using SteamTradeHelper.Context.Models;
using SteamTradeHelper.Dtos;

namespace SteamTradeHelper.Mappings.Profiles.Bots
{
    public class BotsDbToDtoProfile : Profile
    {
        public BotsDbToDtoProfile()
        {
            CreateMap<Bot, BotDto>()
                .ForPath(dest => dest.LastSync, opt => opt.MapFrom(
                     src => src.UpdatedAt));
        }
    }
}
