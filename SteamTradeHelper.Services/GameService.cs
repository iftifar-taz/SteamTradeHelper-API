using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SteamTradeHelper.Context.Models;
using SteamTradeHelper.Dtos;
using SteamTradeHelper.Repositories.Contracts;
using SteamTradeHelper.Services.Contracts;
using SteamTradeHelper.Utilities.Exceptions;

namespace SteamTradeHelper.Services
{
    public class GameService(IBaseRepository<Game> gameRepository, IMapper mapper) : IGameService
    {
        private readonly IBaseRepository<Game> gameRepository = gameRepository;
        private readonly IMapper mapper = mapper;

        public async Task<ListResponse<GameDto>> GetAll()
        {
            var query = gameRepository.GetQueryable();
            query = query.Include(x => x.Cards);
            var games = await gameRepository.GetAllQuery(query);
            if (!games.Any())
            {
                throw new EmptyListException();
            }

            return new ListResponse<GameDto>()
            {
                List = mapper.Map<IEnumerable<Game>, IReadOnlyCollection<GameDto>>(games),
                Total = games.Count(),
                LastSynced = games.Max(x => x.UpdatedAt),
            };
        }

        public async Task<GameDto> Get(int gameId)
        {
            var query = gameRepository.GetQueryable();
            query = query.Include(x => x.Cards);
            var game = await gameRepository.GetByIdQuery(gameId, query) ?? throw new EmptyItemException();
            return mapper.Map<Game, GameDto>(game);
        }

        public async Task SetTradeability()
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

        public async Task SetTradeability(int gameId)
        {
            var query = gameRepository.GetQueryable();
            query = query.Include(x => x.Cards);
            var game = await gameRepository.GetByIdQuery(gameId, query) ?? throw new EmptyItemException();

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
