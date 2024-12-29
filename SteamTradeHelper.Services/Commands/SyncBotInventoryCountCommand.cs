using MediatR;

namespace SteamTradeHelper.Services.Commands
{
    public class SyncBotInventoryCountCommand(int botId) : IRequest
    {
        public int BotId { get; private set; } = botId;
    }
}
