using System.Diagnostics.CodeAnalysis;

namespace SteamTradeHelper.Client.Models.Games
{
    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Legacy code")]
    public class GamesResponse
    {
        public GamesSteamResponse? response { get; set; }
    }
}
