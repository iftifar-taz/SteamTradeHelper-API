using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamTradeHelper.Client.Models.Inventories
{
    public class InventoryAsset
    {
        public int appid { get; set; }

        public string contextid { get; set; }

        public string assetid { get; set; }

        public string classid { get; set; }

        public string instanceid { get; set; }

        public string amount { get; set; }
    }
}
