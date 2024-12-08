using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SteamTradeHelper.Context.Models;
using SteamTradeHelper.Dtos;
using SteamTradeHelper.Repositories.Contracts;
using SteamTradeHelper.Services.Contracts;
using SteamTradeHelper.Utilities.Exceptions;

namespace SteamTradeHelper.Services
{
    public class CardService : ICardService
    {
        private readonly IBaseRepository<Game> gameRepository;
        private readonly IBaseRepository<Card> cardRepository;
        private readonly IMapper mapper;

        public CardService(
            IBaseRepository<Game> gameRepository,
            IBaseRepository<Card> cardRepository,
            IMapper mapper)
        {
            this.gameRepository = gameRepository;
            this.cardRepository = cardRepository;
            this.mapper = mapper;
        }

        public async Task<ListResponse<CardDto>> GetAll()
        {
            var cards = await this.cardRepository.GetAll();
            if (!cards.Any())
            {
                throw new EmptyListException();
            }

            return new ListResponse<CardDto>()
            {
                List = this.mapper.Map<IEnumerable<Card>, IReadOnlyCollection<CardDto>>(cards),
                Total = cards.Count(),
                LastSynced = cards.Max(x => x.UpdatedAt),
            };
        }

        public async Task<ListResponse<CardDto>> GetAll(int gameId)
        {
            var query = this.cardRepository.GetQueryable();
            query = query.Where(x => x.GameId == gameId);
            var cards = await this.cardRepository.GetAllQuery(query);
            if (!cards.Any())
            {
                throw new EmptyListException();
            }

            return new ListResponse<CardDto>()
            {
                List = this.mapper.Map<IEnumerable<Card>, IReadOnlyCollection<CardDto>>(cards),
                Total = cards.Count(),
                LastSynced = cards.Max(x => x.UpdatedAt),
            };
        }

        public async Task SetTradeability(int gameId)
        {
            var game = await this.gameRepository.GetById(gameId);
            game.IsTradeable = false;
            var cardQuery = this.cardRepository.GetQueryable();
            cardQuery = cardQuery.Where(x => x.GameId == game.Id);
            var cards = await this.cardRepository.GetAllQuery(cardQuery);
            foreach (var card in cards)
            {
                card.IsBuyTradeable = false;
                card.IsSellTradeable = false;
            }

            if (game == null)
            {
                throw new EmptyItemException();
            }

            if (!cards.Any())
            {
                throw new EmptyListException();
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
    }
}
