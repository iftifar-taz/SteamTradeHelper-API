namespace SteamTradeHelper.Dtos
{
    public class GameDto
    {
        public int Id { get; set; }

        public int AppId { get; set; }

        public string Name { get; set; }

        public string LogoHash { get; set; }

        public bool IsTradeable { get; set; }

        public int CardCount { get; set; }

        public DateTime LastPriceSync { get; set; }

        public DateTime LastBotSync { get; set; }
    }
}
