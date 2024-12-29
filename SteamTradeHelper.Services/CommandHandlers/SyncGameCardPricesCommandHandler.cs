using MediatR;
using SteamTradeHelper.Client.Contracts;
using SteamTradeHelper.Repositories.Contracts;
using SteamTradeHelper.Services.Commands;
using SteamTradeHelper.Utilities.Exceptions;

namespace SteamTradeHelper.Services.CommandHandlers
{
    public class SyncGameCardPricesCommandHandler(IUnitOfWork unitOfWork, ISteamClient client) : IRequestHandler<SyncGameCardPricesCommand>
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        private readonly ISteamClient client = client;

        public async Task Handle(SyncGameCardPricesCommand request, CancellationToken cancellationToken)
        {
            var game = await unitOfWork.GameRepository.GetById(request.GameId) ?? throw new EmptyItemException();
            var cardQuery = unitOfWork.CardRepository.GetQueryable();
            cardQuery = cardQuery.Where(x => x.GameId == game.Id);
            var cards = await unitOfWork.CardRepository.GetAllQuery(cardQuery);
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

            unitOfWork.CardRepository.PutAll(cards);
            game.UpdatedAt = DateTime.UtcNow;
            unitOfWork.GameRepository.Put(game);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
