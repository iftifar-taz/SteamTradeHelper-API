using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SteamTradeHelper.Context.Models;
using SteamTradeHelper.Dtos;

namespace SteamTradeHelper.Mappings.Profiles.Games
{
    public class GamesDbToDtoProfile : Profile
    {
        public GamesDbToDtoProfile()
        {
            this.CreateMap<Game, GameDto>()
                .ForPath(dest => dest.CardCount, opt => opt.MapFrom(
                     src => src.Cards.Count))
                .ForPath(dest => dest.LastPriceSync, opt => opt.MapFrom(
                     src => src.Cards.Any() ? src.Cards.Max(x => x.UpdatedAt) : DateTime.MinValue))
                .ForPath(dest => dest.LastBotSync, opt => opt.MapFrom(
                     src => DateTime.MinValue));
        }
    }
}
