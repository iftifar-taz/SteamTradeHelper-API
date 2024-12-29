using SteamTradeHelper.Dtos;

namespace SteamTradeHelper.Services.Contracts
{
    public interface ICardService
    {
        Task<ListResponse<CardDto>> GetAll();

        Task<ListResponse<CardDto>> GetAll(int gameId);

        Task SetTradeability(int gameId);
    }
}
