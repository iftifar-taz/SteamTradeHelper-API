using SteamTradeHelper.Dtos;

namespace SteamTradeHelper.Services.Contracts
{
    public interface IBotService
    {
        Task<ListResponse<BotDto>> GetAll();

        Task<BotDto> Get(int botId);
    }
}
