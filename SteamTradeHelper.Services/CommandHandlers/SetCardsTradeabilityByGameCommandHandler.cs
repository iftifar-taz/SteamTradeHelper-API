using MediatR;
using SteamTradeHelper.Repositories.Contracts;
using SteamTradeHelper.Services.Commands;
using SteamTradeHelper.Utilities.Exceptions;

namespace SteamTradeHelper.Services.CommandHandlers
{
    public class SetCardsTradeabilityByGameCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<SetCardsTradeabilityByGameCommand>
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;

        public async Task Handle(SetCardsTradeabilityByGameCommand request, CancellationToken cancellationToken)
        {
            var game = await unitOfWork.GameRepository.GetById(request.GameId) ?? throw new EmptyItemException();
            game.IsTradeable = false;
            var cardQuery = unitOfWork.CardRepository.GetQueryable();
            cardQuery = cardQuery.Where(x => x.GameId == game.Id);
            var cards = await unitOfWork.CardRepository.GetAllQuery(cardQuery);
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

            unitOfWork.CardRepository.PutAll(cards);
            game.UpdatedAt = DateTime.UtcNow;
            unitOfWork.GameRepository.Put(game);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
