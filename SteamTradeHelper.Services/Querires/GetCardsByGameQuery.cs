using MediatR;
using SteamTradeHelper.Dtos;

namespace SteamTradeHelper.Services.Querires
{
    public class GetCardsByGameQuery(int gameId) : IRequest<ListResponse<CardDto>>
    {
        public int GameId { get; private set; } = gameId;
    }
}
