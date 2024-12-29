using MediatR;
using Microsoft.EntityFrameworkCore;
using SteamTradeHelper.Repositories.Contracts;
using SteamTradeHelper.Services.Commands;
using SteamTradeHelper.Utilities.Exceptions;

namespace SteamTradeHelper.Services.CommandHandlers
{
    public class SetGameTradeabilityCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<SetGameTradeabilityCommand>
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;

        public async Task Handle(SetGameTradeabilityCommand request, CancellationToken cancellationToken)
        {
            var query = unitOfWork.GameRepository.GetQueryable();
            query = query.Include(x => x.Cards);
            var game = await unitOfWork.GameRepository.GetByIdQuery(request.GameId, query) ?? throw new EmptyItemException();

            if (game?.Cards?.Count != 0)
            {
                throw new EmptyListException();
            }

            var minCardBuyPrice = game.Cards.Min(x => x.BuyPrice);
            var maxCardSellPrice = game.Cards.Max(x => x.SellPrice);
            game.IsTradeable = maxCardSellPrice > minCardBuyPrice * 1.15;
            unitOfWork.GameRepository.Put(game);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
