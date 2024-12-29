namespace SteamTradeHelper.Services.Contracts
{
    public interface ISteamSyncService
    {
        Task SyncGames();

        Task SyncGameCards(int gameId);

        Task SyncCardPrices(int gameId);

        Task SyncBots();

        Task SyncBotInventoryCount(int botId);
    }
}
