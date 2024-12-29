using AutoMapper;
using SteamTradeHelper.Context.Models;
using SteamTradeHelper.Dtos;

namespace SteamTradeHelper.Mappings.Profiles.Games
{
    public class GamesDbToDtoProfile : Profile
    {
        public GamesDbToDtoProfile()
        {
            CreateMap<Game, GameDto>()
                .ForPath(dest => dest.CardCount, opt => opt.MapFrom(
                     src => src.Cards != null ? src.Cards.Count : 0))
                .ForPath(dest => dest.LastPriceSync, opt => opt.MapFrom(
                     src => src.Cards != null && src.Cards.Any() ? src.Cards.Max(x => x.UpdatedAt) : DateTime.MinValue))
                .ForPath(dest => dest.LastBotSync, opt => opt.MapFrom(
                     src => DateTime.MinValue));
        }
    }
}
