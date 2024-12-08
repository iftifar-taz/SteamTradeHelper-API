using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamTradeHelper.Client.Models.Inventories
{
    public class InventoryResponse
    {
        public IReadOnlyCollection<InventoryAsset> assets { get; set; }

        public IReadOnlyCollection<InventoryDescription> descriptions { get; set; }

        public bool more_items { get; set; }

        public string last_assetid { get; set; }

        public int total_inventory_count { get; set; }

        public bool success { get; set; }

        public int rwgrsn { get; set; }
    }
}
