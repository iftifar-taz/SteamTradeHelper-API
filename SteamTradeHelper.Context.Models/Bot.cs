using System.ComponentModel.DataAnnotations;

namespace SteamTradeHelper.Context.Models
{
    public class Bot : Base
    {
        public required string SteamId { get; set; }

        public string? Name { get; set; }

        public string? LogoUrl { get; set; }

        public string? TradeType { get; set; }

        public string? TradeLink { get; set; }

        public int? InventoryCount { get; set; }

        public bool IsTradingBackground { get; set; }

        public bool IsTradingEmotion { get; set; }

        public bool IsTradingCard { get; set; }

        public bool IsTradingFoilCard { get; set; }
    }
}
