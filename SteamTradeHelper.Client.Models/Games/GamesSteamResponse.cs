using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamTradeHelper.Client.Models.Games
{
    public class GamesSteamResponse
    {
        public int game_count { get; set; }

        public IReadOnlyCollection<GameInformation> games { get; set; }
    }
}
