using SteamTradeHelper.Client.Models.Games;
using SteamTradeHelper.Client.Models.Inventories;
using SteamTradeHelper.Client.Models.ItemPrice;
using SteamTradeHelper.Client.Models.Steam;

namespace SteamTradeHelper.Client.Contracts
{
    public interface ISteamClient
    {
        Task<SwaggerResponse<GamesResponse>> GetSteamGamesAsync(string steamId, string key);

        Task<string> GetSteamGameCardsPageAsync(int appId);

        Task<string> GetSteamCardPageAsync(string steamAppId, int appId, string cardName);

        Task<SwaggerResponse<ItemPriceResponse>> GetItemPriceInformation(int itemId);

        Task<string> GetBotsPageAsync();

        Task<SwaggerResponse<InventoryResponse>> GetSteamInventoryCountAsync(string steamId);
    }
}
