using MediatR;

namespace SteamTradeHelper.Services.Commands
{
    public class SyncGameCardPricesCommand(int gameId) : IRequest
    {
        public int GameId { get; private set; } = gameId;
    }
}
