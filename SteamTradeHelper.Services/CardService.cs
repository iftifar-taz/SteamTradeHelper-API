using AutoMapper;
using SteamTradeHelper.Context.Models;
using SteamTradeHelper.Dtos;
using SteamTradeHelper.Repositories.Contracts;
using SteamTradeHelper.Services.Contracts;
using SteamTradeHelper.Utilities.Exceptions;

namespace SteamTradeHelper.Services
{
    public class CardService(
        IBaseRepository<Game> gameRepository,
        IBaseRepository<Card> cardRepository,
        IMapper mapper) : ICardService
    {
        private readonly IBaseRepository<Game> gameRepository = gameRepository;
        private readonly IBaseRepository<Card> cardRepository = cardRepository;
        private readonly IMapper mapper = mapper;

        public async Task<ListResponse<CardDto>> GetAll()
        {
            var cards = await cardRepository.GetAll();
            if (!cards.Any())
            {
                throw new EmptyListException();
            }

            return new ListResponse<CardDto>()
            {
                List = mapper.Map<IEnumerable<Card>, IReadOnlyCollection<CardDto>>(cards),
                Total = cards.Count(),
                LastSynced = cards.Max(x => x.UpdatedAt),
            };
        }

        public async Task<ListResponse<CardDto>> GetAll(int gameId)
        {
            var query = cardRepository.GetQueryable();
            query = query.Where(x => x.GameId == gameId);
            var cards = await cardRepository.GetAllQuery(query);
            if (!cards.Any())
            {
                throw new EmptyListException();
            }

            return new ListResponse<CardDto>()
            {
                List = mapper.Map<IEnumerable<Card>, IReadOnlyCollection<CardDto>>(cards),
                Total = cards.Count(),
                LastSynced = cards.Max(x => x.UpdatedAt),
            };
        }

        public async Task SetTradeability(int gameId)
        {
            var game = await gameRepository.GetById(gameId) ?? throw new EmptyItemException();
            game.IsTradeable = false;
            var cardQuery = cardRepository.GetQueryable();
            cardQuery = cardQuery.Where(x => x.GameId == game.Id);
            var cards = await cardRepository.GetAllQuery(cardQuery);
            if (!cards.Any())
            {
                throw new EmptyListException();
            }

            foreach (var card in cards)
            {
                card.IsBuyTradeable = false;
                card.IsSellTradeable = false;
            }

            var minCardBuyPrice = cards.Min(x => x.BuyPrice);
            var tradableCards = cards.Where(x => x.SellPrice > minCardBuyPrice * 1.15);
            if (tradableCards.Any())
            {
                var minCard = cards.FirstOrDefault(x => x.BuyPrice == minCardBuyPrice) ?? throw new EmptyItemException();
                minCard.IsBuyTradeable = true;
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
    }
}
