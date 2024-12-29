using MediatR;
using Microsoft.EntityFrameworkCore;
using SteamTradeHelper.Context.Models;
using SteamTradeHelper.Repositories.Contracts;
using SteamTradeHelper.Services.Commands;
using SteamTradeHelper.Utilities.Exceptions;

namespace SteamTradeHelper.Services.CommandHandlers
{
    public class SetGamesTradeabilityCommandHandler(IBaseRepository<Game> gameRepository) : IRequestHandler<SetGamesTradeabilityCommand>
    {
        private readonly IBaseRepository<Game> gameRepository = gameRepository;

        public async Task Handle(SetGamesTradeabilityCommand request, CancellationToken cancellationToken)
        {
            var query = gameRepository.GetQueryable();
            query = query.Include(x => x.Cards);
            var games = await gameRepository.GetAllQuery(query);
            if (!games.Any())
            {
                throw new EmptyListException();
            }

            var gamesWithCards = games.Where(x => x.Cards is not null && x.Cards.Count != 0);
            foreach (var gameWithCards in gamesWithCards)
            {
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                var minCardBuyPrice = gameWithCards.Cards?.Min(x => x.BuyPrice);
                var maxCardSellPrice = gameWithCards.Cards?.Max(x => x.SellPrice);
                gameWithCards.IsTradeable = maxCardSellPrice > minCardBuyPrice * 1.15;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore IDE0079 // Remove unnecessary suppression
            }

            await gameRepository.PutAll(gamesWithCards);
        }
    }
}
