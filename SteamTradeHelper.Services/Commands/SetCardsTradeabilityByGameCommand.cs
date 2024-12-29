using MediatR;

namespace SteamTradeHelper.Services.Commands
{
    public class SetCardsTradeabilityByGameCommand(int gameId) : IRequest
    {
        public int GameId { get; private set; } = gameId;
    }
}
