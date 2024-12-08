using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SteamTradeHelper.Client.Models.Bots;
using SteamTradeHelper.Context.Models;

namespace SteamTradeHelper.Mappings.Profiles.Bots
{
    public class BotsAsfClientToDbProfile : Profile
    {
        public BotsAsfClientToDbProfile()
        {
            this.CreateMap<AsfBotResponse, Bot>();
        }
    }
}
