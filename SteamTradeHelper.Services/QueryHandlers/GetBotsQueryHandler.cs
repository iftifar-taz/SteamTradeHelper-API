using AutoMapper;
using MediatR;
using SteamTradeHelper.Context.Models;
using SteamTradeHelper.Dtos;
using SteamTradeHelper.Repositories.Contracts;
using SteamTradeHelper.Services.Querires;
using SteamTradeHelper.Utilities.Exceptions;

namespace SteamTradeHelper.Services.QueryHandlers
{
    public class GetBotsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetBotsQuery, ListResponse<BotDto>>
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        private readonly IMapper mapper = mapper;

        public async Task<ListResponse<BotDto>> Handle(GetBotsQuery request, CancellationToken cancellationToken)
        {
            var bots = await unitOfWork.BotRepository.GetAll();
            if (!bots.Any())
            {
                throw new EmptyListException();
            }

            return new ListResponse<BotDto>()
            {
                List = mapper.Map<IEnumerable<Bot>, IReadOnlyCollection<BotDto>>(bots),
                Total = bots.Count(),
                LastSynced = bots.Max(x => x.UpdatedAt),
            };
        }
    }
}
