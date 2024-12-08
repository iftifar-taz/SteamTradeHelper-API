using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamTradeHelper.Client.Models.Games
{
    public class GameInformation
    {
        public int appid { get; set; }

        public string name { get; set; }

        public int playtime_forever { get; set; }

        public string img_icon_url { get; set; }

        public string img_logo_url { get; set; }

        public int playtime_windows_forever { get; set; }

        public int playtime_mac_forever { get; set; }

        public int playtime_linux_forever { get; set; }
    }
}
