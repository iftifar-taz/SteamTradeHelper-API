using MediatR;
using SteamTradeHelper.Client.Contracts;
using SteamTradeHelper.Context.Models;
using SteamTradeHelper.Repositories.Contracts;
using SteamTradeHelper.Services.Commands;
using SteamTradeHelper.Utilities.Exceptions;

namespace SteamTradeHelper.Services.CommandHandlers
{
    public class SyncGameCardPricesCommandHandler(IBaseRepository<Game> gameRepository, IBaseRepository<Card> cardRepository, ISteamClient client) : IRequestHandler<SyncGameCardPricesCommand>
    {
        private readonly IBaseRepository<Game> gameRepository = gameRepository;
        private readonly IBaseRepository<Card> cardRepository = cardRepository;
        private readonly ISteamClient client = client;

        public async Task Handle(SyncGameCardPricesCommand request, CancellationToken cancellationToken)
        {
            var game = await gameRepository.GetById(request.GameId) ?? throw new EmptyItemException();
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
    }
}
