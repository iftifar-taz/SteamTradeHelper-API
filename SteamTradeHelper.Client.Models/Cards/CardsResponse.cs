namespace SteamTradeHelper.Client.Models.Cards
{
    public class CardsResponse
    {
        public int GameId { get; set; }

        public int ItemId { get; set; }

        public string? Name { get; set; }

        public int InventoryCount { get; set; }

        public string? LogoUrl { get; set; }
    }
}
