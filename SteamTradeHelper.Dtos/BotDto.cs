namespace SteamTradeHelper.Dtos
{
    public class BotDto
    {
        public int Id { get; set; }

        public required string SteamId { get; set; }

        public string? LogoUrl { get; set; }

        public required string Name { get; set; }

        public string? TradeLink { get; set; }

        public string? TradeType { get; set; }

        public bool IsTradingBackground { get; set; }

        public bool IsTradingEmotions { get; set; }

        public bool IsTradingCard { get; set; }

        public bool IsTradingFoil { get; set; }

        public int? InventoryCount { get; set; }

        public DateTime LastSync { get; set; }
    }
}
