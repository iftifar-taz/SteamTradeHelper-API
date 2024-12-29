using MediatR;

namespace SteamTradeHelper.Services.Commands
{
    public class SyncGameCardsCommand(int gameId) : IRequest
    {
        public int GameId { get; private set; } = gameId;
    }
}
