using System.Diagnostics.CodeAnalysis;

namespace SteamTradeHelper.Client.Models.Inventories
{
    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Legacy code")]
    public class InventoryTag
    {
        public string? category { get; set; }

        public string? internal_name { get; set; }

        public string? localized_category_name { get; set; }

        public string? localized_tag_name { get; set; }
    }
}
