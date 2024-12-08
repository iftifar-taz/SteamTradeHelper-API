using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SteamTradeHelper.Client.Models.Cards;
using SteamTradeHelper.Context.Models;

namespace SteamTradeHelper.Mappings.Profiles.Cards
{
    public class CardsClientToDbProfile : Profile
    {
        public CardsClientToDbProfile()
        {
            this.CreateMap<CardsResponse, Card>();
        }
    }
}
