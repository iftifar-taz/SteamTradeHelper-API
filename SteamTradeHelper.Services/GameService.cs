using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SteamTradeHelper.Context.Models;
using SteamTradeHelper.Dtos;
using SteamTradeHelper.Repositories.Contracts;
using SteamTradeHelper.Services.Contracts;
using SteamTradeHelper.Utilities.Exceptions;

namespace SteamTradeHelper.Services
{
    public class GameService : IGameService
    {
        private readonly IBaseRepository<Game> gameRepository;
        private readonly IMapper mapper;

        public GameService(IBaseRepository<Game> gameRepository, IMapper mapper)
        {
            this.gameRepository = gameRepository;
            this.mapper = mapper;
        }

        public async Task<ListResponse<GameDto>> GetAll()
        {
            var query = this.gameRepository.GetQueryable();
            query = query.Include(x => x.Cards);
            var games = await this.gameRepository.GetAllQuery(query);
            if (!games.Any())
            {
                throw new EmptyListException();
            }

            return new ListResponse<GameDto>()
            {
                List = this.mapper.Map<IEnumerable<Game>, IReadOnlyCollection<GameDto>>(games),
                Total = games.Count(),
                LastSynced = games.Max(x => x.UpdatedAt),
            };
        }

        public async Task<GameDto> Get(int gameId)
        {
            var query = this.gameRepository.GetQueryable();
            query = query.Include(x => x.Cards);
            var game = await this.gameRepository.GetByIdQuery(gameId, query);
            if (game == null)
            {
                throw new EmptyItemException();
            }

            return this.mapper.Map<Game, GameDto>(game);
        }

        public async Task SetTradeability()
        {
            var query = this.gameRepository.GetQueryable();
            query = query.Include(x => x.Cards);
            var games = await this.gameRepository.GetAllQuery(query);
            if (!games.Any())
            {
                throw new EmptyListException();
            }

            var gamesWithCards = games.Where(x => x.Cards.Any());
            foreach (var gameWithCards in gamesWithCards)
            {
                var minCardBuyPrice = gameWithCards.Cards.Min(x => x.BuyPrice);
                var maxCardSellPrice = gameWithCards.Cards.Max(x => x.SellPrice);
                gameWithCards.IsTradeable = maxCardSellPrice > minCardBuyPrice * 1.15;
            }

            await this.gameRepository.PutAll(gamesWithCards);
        }

        public async Task SetTradeability(int gameId)
        {
            var query = this.gameRepository.GetQueryable();
            query = query.Include(x => x.Cards);
            var game = await this.gameRepository.GetByIdQuery(gameId, query);
            if (game == null)
            {
                throw new EmptyItemException();
            }

            if (!game.Cards.Any())
            {
                throw new EmptyListException();
            }

            var minCardBuyPrice = game.Cards.Min(x => x.BuyPrice);
            var maxCardSellPrice = game.Cards.Max(x => x.SellPrice);
            game.IsTradeable = maxCardSellPrice > minCardBuyPrice * 1.15;
            await this.gameRepository.Put(game);
        }
    }
}
