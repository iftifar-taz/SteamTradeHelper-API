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
    public class GetGameQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetGameQuery, GameDto>
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        private readonly IMapper mapper = mapper;

        public async Task<GameDto> Handle(GetGameQuery request, CancellationToken cancellationToken)
        {
            var query = unitOfWork.GameRepository.GetQueryable();
            query = query.Include(x => x.Cards);
            var game = await unitOfWork.GameRepository.GetByIdQuery(request.GameId, query) ?? throw new EmptyItemException();
            return mapper.Map<Game, GameDto>(game);
        }
    }
}
