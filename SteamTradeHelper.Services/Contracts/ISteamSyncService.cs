using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
