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
    public class SteamSyncService : ISteamSyncService
    {
        private readonly SteamSettings steamSettings;
        private readonly IBaseRepository<Game> gameRepository;
        private readonly IBaseRepository<Card> cardRepository;
        private readonly IBaseRepository<Bot> botRepository;
        private readonly IMapper mapper;
        private readonly ISteamClient client;

        public SteamSyncService(
            IOptions<SteamSettings> options,
            IBaseRepository<Game> gameRepository,
            IBaseRepository<Card> cardRepository,
            IBaseRepository<Bot> botRepository,
            IMapper mapper,
            ISteamClient client)
        {
            this.steamSettings = options.Value;
            this.gameRepository = gameRepository;
            this.cardRepository = cardRepository;
            this.botRepository = botRepository;
            this.mapper = mapper;
            this.client = client;
        }

        public async Task SyncGames()
        {
            var existingGames = await this.gameRepository.GetAll();
            var existingGameAppIds = existingGames.Select(x => x.AppId);
            var response = await this.client.GetSteamGamesAsync(this.steamSettings.SteamId, this.steamSettings.SteamKey);
            var tempResult = this.mapper.Map<IEnumerable<GameInformation>, IEnumerable<Game>>(response.Result.response.games);
            var result = tempResult.Where(x => !existingGameAppIds.Contains(x.AppId));
            await this.gameRepository.SaveAll(result);
        }

        public async Task SyncGameCards(int gameId)
        {
            var game = await this.gameRepository.GetById(gameId);
            var cardQuery = this.cardRepository.GetQueryable();
            cardQuery = cardQuery.Where(x => x.GameId == game.Id);
            var existingCards = await this.cardRepository.GetAllQuery(cardQuery);
            if (game == null)
            {
                throw new EmptyItemException();
            }

            if (existingCards.Any())
            {
                return;
            }

            var cardsHtmlPage = await this.client.GetSteamGameCardsPageAsync(game.AppId);
            var cardsInformation = HtmlPerser.GetGameCards(cardsHtmlPage);
            foreach (var cardInformation in cardsInformation)
            {
                var realCardNameHtmlPage = await this.client.GetSteamCardPageAsync(this.steamSettings.SteamAppId, game.AppId, cardInformation.Name);
                var cardName = HtmlPerser.GetCardName(realCardNameHtmlPage);
                if (cardName.StartsWith(game.AppId.ToString()))
                {
                    cardInformation.Name = $"{cardInformation.Name} (Trading Card)";
                }

                var cardHtmlPage = await this.client.GetSteamCardPageAsync(this.steamSettings.SteamAppId, game.AppId, cardInformation.Name);
                cardInformation.ItemId = HtmlPerser.GetItemId(cardHtmlPage);
                cardInformation.GameId = game.Id;
            }

            var cards = this.mapper.Map<IEnumerable<CardsResponse>, IEnumerable<Card>>(cardsInformation);

            if (cards.Count() != 0)
            {
                await this.cardRepository.SaveAll(cards);
                game.UpdatedAt = DateTime.UtcNow;
                await this.gameRepository.Put(game);
            }
        }

        public async Task SyncCardPrices(int gameId)
        {
            var game = await this.gameRepository.GetById(gameId);
            var cardQuery = this.cardRepository.GetQueryable();
            cardQuery = cardQuery.Where(x => x.GameId == game.Id);
            var cards = await this.cardRepository.GetAllQuery(cardQuery);
            if (game == null)
            {
                throw new EmptyItemException();
            }

            if (!cards.Any())
            {
                throw new EmptyListException();
            }

            foreach (var card in cards)
            {
                var itemPriceInformation = await this.client.GetItemPriceInformation(card.ItemId);

                card.BuyPrice = Convert.ToInt32(itemPriceInformation.Result.lowest_sell_order);
                card.SellPrice = Convert.ToInt32(itemPriceInformation.Result.highest_buy_order);
            }

            var minCardBuyPrice = cards.Min(x => x.BuyPrice);
            var tradableCards = cards.Where(x => x.SellPrice > minCardBuyPrice * 1.15);
            if (tradableCards.Any())
            {
                var minCard = cards.FirstOrDefault(x => x.BuyPrice == minCardBuyPrice);
                minCard.IsBuyTradeable = true;
                foreach (var tradableCard in tradableCards)
                {
                    tradableCard.IsSellTradeable = true;
                }

                game.IsTradeable = true;
            }

            await this.cardRepository.PutAll(cards);
            game.UpdatedAt = DateTime.UtcNow;
            await this.gameRepository.Put(game);
        }

        public async Task SyncBots()
        {
            var existingBots = await this.botRepository.GetAll();
            var existingBotSteamIds = existingBots.Select(x => x.SteamId);
            var botsHtmlPage = await this.client.GetBotsPageAsync();
            var botsClientResponse = HtmlPerser.GetBots(botsHtmlPage);
            var botsClient = botsClientResponse.Where(x => !existingBotSteamIds.Contains(x.SteamId));

            // TODO: fix not found logo urls(img/default.jpg) with steam api call https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v2/?key=0B71FB46568A596BDA0D7DD520914701&steamids=1,2,3,4,5
            var bots = this.mapper.Map<IEnumerable<AsfBotResponse>, IEnumerable<Bot>>(botsClient);
            await this.botRepository.SaveAll(bots);
        }

        public async Task SyncBotInventoryCount(int botId)
        {
            var bot = await this.botRepository.GetById(botId);
            var response = await this.client.GetSteamInventoryCountAsync(bot.SteamId);
            bot.InventoryCount = response.Result.total_inventory_count;
            await this.botRepository.Put(bot);
        }
    }
}
