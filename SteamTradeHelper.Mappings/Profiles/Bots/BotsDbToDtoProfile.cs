﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SteamTradeHelper.Context.Models;
using SteamTradeHelper.Dtos;

namespace SteamTradeHelper.Mappings.Profiles.Bots
{
    public class BotsDbToDtoProfile : Profile
    {
        public BotsDbToDtoProfile()
        {
            this.CreateMap<Bot, BotDto>()
                .ForPath(dest => dest.LastSync, opt => opt.MapFrom(
                     src => src.UpdatedAt));
        }
    }
}