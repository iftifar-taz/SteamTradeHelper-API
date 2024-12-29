using MediatR;
using SteamTradeHelper.Dtos;

namespace SteamTradeHelper.Services.Querires
{
    public class GetGamesQuery : IRequest<ListResponse<GameDto>>
    {
    }
}
