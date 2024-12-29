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
    public class GetGamesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetGamesQuery, ListResponse<GameDto>>
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        private readonly IMapper mapper = mapper;

        public async Task<ListResponse<GameDto>> Handle(GetGamesQuery request, CancellationToken cancellationToken)
        {
            var query = unitOfWork.GameRepository.GetQueryable();
            query = query.Include(x => x.Cards);
            var games = await unitOfWork.GameRepository.GetAllQuery(query);
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
    }
}
