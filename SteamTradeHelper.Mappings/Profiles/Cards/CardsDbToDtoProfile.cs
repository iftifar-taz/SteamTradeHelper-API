﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SteamTradeHelper.Context.Models;
using SteamTradeHelper.Dtos;

namespace SteamTradeHelper.Mappings.Profiles.Cards
{
    public class CardsDbToDtoProfile : Profile
    {
        public CardsDbToDtoProfile()
        {
            this.CreateMap<Card, CardDto>()
                .ForPath(dest => dest.LastPriceSync, opt => opt.MapFrom(
                     src => src.UpdatedAt))
                .ForPath(dest => dest.LastBotSync, opt => opt.MapFrom(
                     src => DateTime.MinValue));
        }
    }
}