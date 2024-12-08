using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamTradeHelper.Client.Models.Inventories
{
    public class InventoryTag
    {
        public string category { get; set; }

        public string internal_name { get; set; }

        public string localized_category_name { get; set; }

        public string localized_tag_name { get; set; }
    }
}
