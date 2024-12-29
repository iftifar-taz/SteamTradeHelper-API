using AutoMapper;
using SteamTradeHelper.Context.Models;
using SteamTradeHelper.Dtos;

namespace SteamTradeHelper.Mappings.Profiles.Cards
{
    public class CardsDbToDtoProfile : Profile
    {
        public CardsDbToDtoProfile()
        {
            CreateMap<Card, CardDto>()
                .ForPath(dest => dest.LastPriceSync, opt => opt.MapFrom(
                     src => src.UpdatedAt))
                .ForPath(dest => dest.LastBotSync, opt => opt.MapFrom(
                     src => DateTime.MinValue));
        }
    }
}
