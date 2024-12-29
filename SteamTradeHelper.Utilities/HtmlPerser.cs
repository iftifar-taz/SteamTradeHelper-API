using HtmlAgilityPack;
using SteamTradeHelper.Client.Models.Bots;
using SteamTradeHelper.Client.Models.Cards;

namespace SteamTradeHelper.Utilities
{
    public static class HtmlPerser
    {
        public static IEnumerable<CardsResponse> GetGameCards(string htmlPage)
        {
            var html = new HtmlDocument();
            html.LoadHtml(htmlPage);
            var cardSectionDiv = html.DocumentNode.Descendants()
                .FirstOrDefault(x => (x.Name == "div" && x.Attributes["class"] != null &&
                   x.Attributes["class"].Value.Contains("badge_card_set_cards")));

            var cardInformations = new List<CardsResponse>();
            if (cardSectionDiv != null)
            {
                cardInformations.AddRange(GetCardInformations(cardSectionDiv, "owned", 3));
                cardInformations.AddRange(GetCardInformations(cardSectionDiv, "unowned", 0));
            }

            return cardInformations;
        }

        public static string GetCardName(string htmlPage)
        {
            var html = new HtmlDocument();
            html.LoadHtml(htmlPage);
            var cardDivs = html.DocumentNode.Descendants()
                .Where(x => x.Name == "div" && x.Attributes["class"] != null &&
                   x.Attributes["class"].Value.Contains("market_listing_nav_container"))
                .ElementAtOrDefault(0)?
                .Descendants().ElementAtOrDefault(1)?
                .Descendants().ElementAtOrDefault(7)?.InnerHtml;

            return cardDivs ?? string.Empty;
        }

        public static int GetItemId(string htmlPage)
        {
            var html = new HtmlDocument();
            html.LoadHtml(htmlPage);
            var itemId = html.DocumentNode.Descendants()
                .LastOrDefault(x => x.Name == "script")?
                .InnerHtml.Split("ItemActivityTicker.Start(")[1]
                .Split(");")[0] ?? string.Empty;

            return Convert.ToInt32(itemId.Trim());
        }

        public static IEnumerable<AsfBotResponse> GetBots(string htmlPage)
        {
            var html = new HtmlDocument();
            html.LoadHtml(htmlPage);
            var userDivs = html.DocumentNode.Descendants()
                .Where(x => (x.Name == "div" && x.Attributes["class"] != null &&
                   x.Attributes["class"].Value == "user"));
            var asfBots = userDivs.Select(x =>
            {
                return new AsfBotResponse
                {
                    SteamId = GetBotSteamId(x),
                    Name = GetBotName(x),
                    LogoUrl = GetBotLogoUrl(x),
                    TradeType = GetBotTradeType(x),
                    TradeLink = GetBotTradeLink(x),
                    IsTradingBackground = GetBotIsTrading(x, "Backgrounds"),
                    IsTradingEmotion = GetBotIsTrading(x, "Emoticons"),
                    IsTradingCard = GetBotIsTrading(x, "Cards"),
                    IsTradingFoilCard = GetBotIsTrading(x, "Foil cards"),
                };
            });

            return asfBots;
        }

        private static IEnumerable<CardsResponse> GetCardInformations(HtmlNode cardSectionDiv, string type, int position)
        {
            var cardDivs = cardSectionDiv.Descendants()
                .Where(x => (x.Name == "div" && x.Attributes["class"] != null &&
                   x.Attributes["class"].Value.Contains($"badge_card_set_card {type}")));

            return cardDivs.Select(x =>
            {
                return new CardsResponse()
                {
                    Name = GetCardName(x, position),
                    LogoUrl = GetCardLogoUrl(x),
                    InventoryCount = GetCardInventoryCount(x),
                };
            });
        }

        private static string GetCardName(HtmlNode node, int position)
        {
            var cardName = node.Descendants()
                .FirstOrDefault(x => (x.Name == "div" && x.Attributes["class"] != null &&
                    x.Attributes["class"].Value.Contains("badge_card_set_text ellipsis") && x.ChildNodes.Count != 1))?
                .Descendants().ElementAtOrDefault(position)?.InnerText.Trim();

            return cardName ?? string.Empty;
        }

        private static string GetCardLogoUrl(HtmlNode node)
        {
            var cardLogo = node.Descendants()
                .FirstOrDefault(x => (x.Name == "img" && x.Attributes["class"] != null &&
                   x.Attributes["class"].Value.Contains("gamecard")))?
                .Attributes["src"].Value;

            return cardLogo ?? string.Empty;
        }

        private static int GetCardInventoryCount(HtmlNode node)
        {
            var cardQuantityDiv = node.Descendants()
                .FirstOrDefault(x => (x.Name == "div" && x.Attributes["class"] != null &&
                   x.Attributes["class"].Value.Contains("badge_card_set_text_qty")));

            if (cardQuantityDiv != null)
            {
                var countString = cardQuantityDiv.InnerHtml;
                return Convert.ToInt32(countString[1..^1]);
            }

            return 0;
        }

        private static string GetBotSteamId(HtmlNode node)
        {
            var steamId = node.Descendants()
                .FirstOrDefault(x => (x.Name == "a" && x.Attributes["href"] != null &&
                   x.Attributes["href"].Value.Contains("https://steamcommunity.com/profiles")))?
                .Attributes["href"].Value
                .Split("/").Last();
            return steamId ?? string.Empty;
        }

        private static string GetBotLogoUrl(HtmlNode node)
        {
            var logoUrl = node.Descendants()
                .FirstOrDefault(x => (x.Name == "img" && x.Attributes["class"] != null &&
                   x.Attributes["class"].Value.Contains("user-image")))?
                .Attributes["src"].Value;
            return logoUrl ?? string.Empty;
        }

        private static string GetBotName(HtmlNode node)
        {
            var name = node.Descendants()
                .FirstOrDefault(x => (x.Name == "div" && x.Attributes["class"] != null &&
                   x.Attributes["class"].Value.Contains("user-nickname")))?
                .Descendants().ElementAtOrDefault(0)?.InnerHtml;
            return name ?? string.Empty;
        }

        private static string GetBotTradeType(HtmlNode node)
        {
            var tradeType = node.Descendants()
                .FirstOrDefault(x => (x.Name == "div" && x.Attributes["class"] != null &&
                   x.Attributes["class"].Value.Contains("badge")))?
                .Descendants().ElementAtOrDefault(0)?.InnerHtml;
            return tradeType ?? string.Empty;
        }

        private static string GetBotTradeLink(HtmlNode node)
        {
            var tradeLink = node.Descendants()
                .FirstOrDefault(x => (x.Name == "a" && x.Attributes["href"] != null &&
                   x.Attributes["href"].Value.Contains("https://steamcommunity.com/tradeoffer/new")))?
                .Attributes["href"].Value;
            return tradeLink ?? string.Empty;
        }

        private static bool GetBotIsTrading(HtmlNode node, string type)
        {
            var isTradingDiv = node.Descendants()
                .FirstOrDefault(x => (x.Name == "img" && x.Attributes["title"] != null && string.Equals(x.Attributes["title"].Value, type, StringComparison.OrdinalIgnoreCase)));
            return isTradingDiv != null;
        }
    }
}
