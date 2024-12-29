using System.Diagnostics.CodeAnalysis;

namespace SteamTradeHelper.Client.Models.Games
{
    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Legacy code")]
    public class GamesSteamResponse
    {
        public int game_count { get; set; }

        public IReadOnlyCollection<GameInformation>? games { get; set; }
    }
}
