using MediatR;
using SteamTradeHelper.Dtos;

namespace SteamTradeHelper.Services.Querires
{
    public class GetBotQuery(int botId) : IRequest<BotDto>
    {
        public int BotId { get; private set; } = botId;
    }
}
