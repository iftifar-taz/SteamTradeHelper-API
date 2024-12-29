﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using SteamTradeHelper.Context.Models;
using SteamTradeHelper.Repositories.Contracts;
using SteamTradeHelper.Services.Commands;
using SteamTradeHelper.Utilities.Exceptions;

namespace SteamTradeHelper.Services.CommandHandlers
{
    public class SetGameTradeabilityCommandHandler(IBaseRepository<Game> gameRepository) : IRequestHandler<SetGameTradeabilityCommand>
    {
        private readonly IBaseRepository<Game> gameRepository = gameRepository;

        public async Task Handle(SetGameTradeabilityCommand request, CancellationToken cancellationToken)
        {
            var query = gameRepository.GetQueryable();
            query = query.Include(x => x.Cards);
            var game = await gameRepository.GetByIdQuery(request.GameId, query) ?? throw new EmptyItemException();

            if (game?.Cards?.Count != 0)
            {
                throw new EmptyListException();
            }

            var minCardBuyPrice = game.Cards.Min(x => x.BuyPrice);
            var maxCardSellPrice = game.Cards.Max(x => x.SellPrice);
            game.IsTradeable = maxCardSellPrice > minCardBuyPrice * 1.15;
            await gameRepository.Put(game);
        }
    }
}