using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamTradeHelper.Client.Models.Bots
{
    public class AsfBotResponse
    {
        public string SteamId { get; set; }

        public string Name { get; set; }

        public string LogoUrl { get; set; }

        public string TradeType { get; set; }

        public string TradeLink { get; set; }

        public bool IsTradingBackground { get; set; }

        public bool IsTradingEmotion { get; set; }

        public bool IsTradingCard { get; set; }

        public bool IsTradingFoilCard { get; set; }
    }
}
