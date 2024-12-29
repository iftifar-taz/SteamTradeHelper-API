namespace SteamTradeHelper.Context.Models
{
    public class Card : Base
    {
        public required int ItemId { get; set; }
        public required string Name { get; set; }

        public string? LogoUrl { get; set; }

        public bool IsBuyTradeable { get; set; }

        public bool IsSellTradeable { get; set; }

        public int InventoryCount { get; set; }

        public int? BuyPrice { get; set; }

        public int? SellPrice { get; set; }

        public required Game Game { get; set; }

        public required int GameId { get; set; }
    }
}
