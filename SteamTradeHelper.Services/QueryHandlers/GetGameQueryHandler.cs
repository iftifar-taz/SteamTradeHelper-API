using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SteamTradeHelper.Context.Models;
using SteamTradeHelper.Dtos;
using SteamTradeHelper.Repositories.Contracts;
using SteamTradeHelper.Services.Querires;
using SteamTradeHelper.Utilities.Exceptions;

namespace SteamTradeHelper.Services.QueryHandlers
{
    public class GetGameQueryHandler(IBaseRepository<Game> gameRepository, IMapper mapper) : IRequestHandler<GetGameQuery, GameDto>
    {
        private readonly IBaseRepository<Game> gameRepository = gameRepository;
        private readonly IMapper mapper = mapper;

        public async Task<GameDto> Handle(GetGameQuery request, CancellationToken cancellationToken)
        {
            var query = gameRepository.GetQueryable();
            query = query.Include(x => x.Cards);
            var game = await gameRepository.GetByIdQuery(request.GameId, query) ?? throw new EmptyItemException();
            return mapper.Map<Game, GameDto>(game);
        }
    }
}
