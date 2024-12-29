using AutoMapper;
using SteamTradeHelper.Client.Models.Games;
using SteamTradeHelper.Context.Models;

namespace SteamTradeHelper.Mappings.Profiles.Games
{
    public class GamesClientToDbProfile : Profile
    {
        public GamesClientToDbProfile()
        {
            CreateMap<GameInformation, Game>()
                .ForPath(dest => dest.Name, opt => opt.MapFrom(
                    src => src.name))
                .ForPath(dest => dest.AppId, opt => opt.MapFrom(
                    src => src.appid))
                .ForPath(dest => dest.IconHash, opt => opt.MapFrom(
                    src => src.img_icon_url))
                .ForPath(dest => dest.LogoHash, opt => opt.MapFrom(
                    src => src.img_logo_url))
                .ForPath(dest => dest.Cards, opt => opt.MapFrom(
                    src => new List<Card>()));
        }
    }
}
