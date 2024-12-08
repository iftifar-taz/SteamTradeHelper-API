using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamTradeHelper.Client.Models.Inventories
{
    public class InventoryDescription
    {
        public int appid { get; set; }

        public string classid { get; set; }

        public string instanceid { get; set; }

        public int currency { get; set; }

        public string background_color { get; set; }

        public string icon_url { get; set; }

        public string icon_url_large { get; set; }

        public IReadOnlyCollection<object> descriptions { get; set; }

        public int tradable { get; set; }

        public string name { get; set; }

        public string type { get; set; }

        public string market_name { get; set; }

        public string market_hash_name { get; set; }

        public int market_fee_app { get; set; }

        public int commodity { get; set; }

        public int market_tradable_restriction { get; set; }

        public int market_marketable_restriction { get; set; }

        public IReadOnlyCollection<InventoryTag> tags { get; set; }
    }
}
