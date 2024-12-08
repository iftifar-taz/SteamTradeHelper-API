namespace SteamTradeHelper.Dtos
{
    public class CardDto
    {
        public int Id { get; set; }

        public int ItemId { get; set; }

        public int GameId { get; set; }

        public string Name { get; set; }

        public string LogoUrl { get; set; }

        public bool IsBuyTradeable { get; set; }

        public bool IsSellTradeable { get; set; }

        public int InventoryCount { get; set; }

        public int? BuyPrice { get; set; }

        public int? SellPrice { get; set; }

        public DateTime LastPriceSync { get; set; }

        public DateTime LastBotSync { get; set; }
    }
}
