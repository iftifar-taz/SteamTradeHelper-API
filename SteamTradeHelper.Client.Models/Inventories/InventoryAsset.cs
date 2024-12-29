using System.Diagnostics.CodeAnalysis;

namespace SteamTradeHelper.Client.Models.Inventories
{
    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Legacy code")]
    public class InventoryAsset
    {
        public int appid { get; set; }

        public string? contextid { get; set; }

        public string? assetid { get; set; }

        public string? classid { get; set; }

        public string? instanceid { get; set; }

        public string? amount { get; set; }
    }
}
