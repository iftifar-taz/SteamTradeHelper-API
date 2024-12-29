using AutoMapper;
using MediatR;
using SteamTradeHelper.Context.Models;
using SteamTradeHelper.Dtos;
using SteamTradeHelper.Repositories.Contracts;
using SteamTradeHelper.Services.Querires;
using SteamTradeHelper.Utilities.Exceptions;

namespace SteamTradeHelper.Services.QueryHandlers
{
    public class GetCardsByGameQueryHandler(IBaseRepository<Card> cardRepository, IMapper mapper) : IRequestHandler<GetCardsByGameQuery, ListResponse<CardDto>>
    {
        private readonly IBaseRepository<Card> cardRepository = cardRepository;
        private readonly IMapper mapper = mapper;

        public async Task<ListResponse<CardDto>> Handle(GetCardsByGameQuery request, CancellationToken cancellationToken)
        {
            var query = cardRepository.GetQueryable();
            query = query.Where(x => x.GameId == request.GameId);
            var cards = await cardRepository.GetAllQuery(query);
            if (!cards.Any())
            {
                throw new EmptyListException();
            }

            return new ListResponse<CardDto>()
            {
                List = mapper.Map<IEnumerable<Card>, IReadOnlyCollection<CardDto>>(cards),
                Total = cards.Count(),
                LastSynced = cards.Max(x => x.UpdatedAt),
            };
        }
    }
}
