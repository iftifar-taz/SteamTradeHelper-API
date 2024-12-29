using SteamTradeHelper.Dtos;

namespace SteamTradeHelper.Services.Contracts
{
    public interface IGameService
    {
        Task<ListResponse<GameDto>> GetAll();

        Task<GameDto> Get(int gameId);

        Task SetTradeability();

        Task SetTradeability(int gameId);
    }
}
