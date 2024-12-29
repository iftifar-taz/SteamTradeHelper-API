using AutoMapper;
using SteamTradeHelper.Client.Models.Cards;
using SteamTradeHelper.Context.Models;

namespace SteamTradeHelper.Mappings.Profiles.Cards
{
    public class CardsClientToDbProfile : Profile
    {
        public CardsClientToDbProfile()
        {
            CreateMap<CardsResponse, Card>();
        }
    }
}
