using MediatR;
using SteamTradeHelper.Context.Models;
using SteamTradeHelper.Repositories.Contracts;
using SteamTradeHelper.Services.Commands;
using SteamTradeHelper.Utilities.Exceptions;

namespace SteamTradeHelper.Services.CommandHandlers
{
    public class SetCardsTradeabilityByGameCommandHandler(IBaseRepository<Game> gameRepository, IBaseRepository<Card> cardRepository) : IRequestHandler<SetCardsTradeabilityByGameCommand>
    {
        private readonly IBaseRepository<Game> gameRepository = gameRepository;
        private readonly IBaseRepository<Card> cardRepository = cardRepository;

        public async Task Handle(SetCardsTradeabilityByGameCommand request, CancellationToken cancellationToken)
        {
            var game = await gameRepository.GetById(request.GameId) ?? throw new EmptyItemException();
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
