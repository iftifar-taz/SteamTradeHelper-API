using MediatR;
using SteamTradeHelper.Dtos;

namespace SteamTradeHelper.Services.Querires
{
    public class GetGameQuery(int gameId) : IRequest<GameDto>
    {
        public int GameId { get; private set; } = gameId;
    }
}
