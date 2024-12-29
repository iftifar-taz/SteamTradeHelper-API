using System.Diagnostics.CodeAnalysis;

namespace SteamTradeHelper.Client.Models.Inventories
{
    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Legacy code")]
    public class InventoryResponse
    {
        public IReadOnlyCollection<InventoryAsset>? assets { get; set; }

        public IReadOnlyCollection<InventoryDescription>? descriptions { get; set; }

        public bool more_items { get; set; }

        public string? last_assetid { get; set; }

        public int total_inventory_count { get; set; }

        public bool success { get; set; }

        public int rwgrsn { get; set; }
    }
}
