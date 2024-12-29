using MediatR;

namespace SteamTradeHelper.Services.Commands
{
    public class SetGameTradeabilityCommand(int gameId) : IRequest
    {
        public int GameId { get; private set; } = gameId;
    }
}
