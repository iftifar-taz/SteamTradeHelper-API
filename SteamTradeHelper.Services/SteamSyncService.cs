using AutoMapper;
using Microsoft.Extensions.Options;
using SteamTradeHelper.Client.Contracts;
using SteamTradeHelper.Client.Models.Bots;
using SteamTradeHelper.Client.Models.Cards;
using SteamTradeHelper.Client.Models.Games;
using SteamTradeHelper.Context.Models;
using SteamTradeHelper.Dtos;
using SteamTradeHelper.Repositories.Contracts;
using SteamTradeHelper.Services.Contracts;
using SteamTradeHelper.Utilities;
using SteamTradeHelper.Utilities.Exceptions;

namespace SteamTradeHelper.Services
{
    public class SteamSyncService(
        IOptions<SteamSettings> options,
        IBaseRepository<Game> gameRepository,
        IBaseRepository<Card> cardRepository,
        IBaseRepository<Bot> botRepository,
        IMapper mapper,
        ISteamClient client) : ISteamSyncService
    {
        private readonly SteamSettings steamSettings = options.Value;
        private readonly IBaseRepository<Game> gameRepository = gameRepository;
        private readonly IBaseRepository<Card> cardRepository = cardRepository;
        private readonly IBaseRepository<Bot> botRepository = botRepository;
        private readonly IMapper mapper = mapper;
        private readonly ISteamClient client = client;

        public async Task SyncGames()
        {
            var existingGames = await gameRepository.GetAll();
            var existingGameAppIds = existingGames.Select(x => x.AppId);
            var response = await client.GetSteamGamesAsync(steamSettings.SteamId, steamSettings.SteamKey);
            var tempResult = mapper.Map<IEnumerable<GameInformation>, IEnumerable<Game>>(response.Result?.response?.games ?? []);
            var result = tempResult.Where(x => !existingGameAppIds.Contains(x.AppId));
            await gameRepository.SaveAll(result);
        }

        public async Task SyncGameCards(int gameId)
        {
            var game = await gameRepository.GetById(gameId) ?? throw new EmptyItemException();
            var cardQuery = cardRepository.GetQueryable();
            cardQuery = cardQuery.Where(x => x.GameId == game.Id);
            var existingCards = await cardRepository.GetAllQuery(cardQuery);

            if (existingCards.Any())
            {
                return;
            }

            var cardsHtmlPage = await client.GetSteamGameCardsPageAsync(game.AppId);
            var cardsInformation = HtmlPerser.GetGameCards(cardsHtmlPage);
            foreach (var cardInformation in cardsInformation)
            {
                var realCardNameHtmlPage = await client.GetSteamCardPageAsync(steamSettings.SteamAppId, game.AppId, cardInformation.Name ?? string.Empty);
                var cardName = HtmlPerser.GetCardName(realCardNameHtmlPage);
                if (cardName.StartsWith(game.AppId.ToString()))
                {
                    cardInformation.Name = $"{cardInformation.Name} (Trading Card)";
                }

                var cardHtmlPage = await client.GetSteamCardPageAsync(steamSettings.SteamAppId, game.AppId, cardInformation.Name ?? string.Empty);
                cardInformation.ItemId = HtmlPerser.GetItemId(cardHtmlPage);
                cardInformation.GameId = game.Id;
            }

            var cards = mapper.Map<IEnumerable<CardsResponse>, IEnumerable<Card>>(cardsInformation);

            if (cards.Any())
            {
                await cardRepository.SaveAll(cards);
                game.UpdatedAt = DateTime.UtcNow;
                await gameRepository.Put(game);
            }
        }

        public async Task SyncCardPrices(int gameId)
        {
            var game = await gameRepository.GetById(gameId) ?? throw new EmptyItemException();
            var cardQuery = cardRepository.GetQueryable();
            cardQuery = cardQuery.Where(x => x.GameId == game.Id);
            var cards = await cardRepository.GetAllQuery(cardQuery);
            if (!cards.Any())
            {
                throw new EmptyListException();
            }

            foreach (var card in cards)
            {
                var itemPriceInformation = await client.GetItemPriceInformation(card.ItemId);

                card.BuyPrice = Convert.ToInt32(itemPriceInformation.Result?.lowest_sell_order);
                card.SellPrice = Convert.ToInt32(itemPriceInformation.Result?.highest_buy_order);
            }

            var minCardBuyPrice = cards.Min(x => x.BuyPrice);
            var tradableCards = cards.Where(x => x.SellPrice > minCardBuyPrice * 1.15);
            if (tradableCards.Any())
            {
                var minCard = cards.FirstOrDefault(x => x.BuyPrice == minCardBuyPrice);
                if (minCard is not null)
                {
                    minCard.IsBuyTradeable = true;
                }

                foreach (var tradableCard in tradableCards)
                {
                    tradableCard.IsSellTradeable = true;
                }

                game.IsTradeable = true;
            }

            await cardRepository.PutAll(cards);
            game.UpdatedAt = DateTime.UtcNow;
            await gameRepository.Put(game);
        }

        public async Task SyncBots()
        {
            var existingBots = await botRepository.GetAll();
            var existingBotSteamIds = existingBots.Select(x => x.SteamId);
            var botsHtmlPage = await client.GetBotsPageAsync();
            var botsClientResponse = HtmlPerser.GetBots(botsHtmlPage);
            var botsClient = botsClientResponse.Where(x => !existingBotSteamIds.Contains(x.SteamId));

            // TODO: fix not found logo urls(img/default.jpg) with steam api call https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v2/?key=0B71FB46568A596BDA0D7DD520914701&steamids=1,2,3,4,5
            var bots = mapper.Map<IEnumerable<AsfBotResponse>, IEnumerable<Bot>>(botsClient);
            await botRepository.SaveAll(bots);
        }

        public async Task SyncBotInventoryCount(int botId)
        {
            var bot = await botRepository.GetById(botId) ?? throw new EmptyItemException();
            var response = await client.GetSteamInventoryCountAsync(bot.SteamId);
            bot.InventoryCount = response.Result?.total_inventory_count;
            await botRepository.Put(bot);
        }
    }
}
