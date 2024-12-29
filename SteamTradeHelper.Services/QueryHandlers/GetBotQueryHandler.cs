using AutoMapper;
using MediatR;
using SteamTradeHelper.Context.Models;
using SteamTradeHelper.Dtos;
using SteamTradeHelper.Repositories.Contracts;
using SteamTradeHelper.Services.Querires;
using SteamTradeHelper.Utilities.Exceptions;

namespace SteamTradeHelper.Services.QueryHandlers
{
    public class GetBotQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetBotQuery, BotDto>
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        private readonly IMapper mapper = mapper;

        public async Task<BotDto> Handle(GetBotQuery request, CancellationToken cancellationToken)
        {
            var bot = await unitOfWork.BotRepository.GetById(request.BotId) ?? throw new EmptyItemException();
            return mapper.Map<Bot, BotDto>(bot);
        }
    }
}
