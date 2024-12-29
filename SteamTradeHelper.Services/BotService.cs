using AutoMapper;
using SteamTradeHelper.Context.Models;
using SteamTradeHelper.Dtos;
using SteamTradeHelper.Repositories.Contracts;
using SteamTradeHelper.Services.Contracts;
using SteamTradeHelper.Utilities.Exceptions;

namespace SteamTradeHelper.Services
{
    public class BotService(IBaseRepository<Bot> botRepository, IMapper mapper) : IBotService
    {
        private readonly IBaseRepository<Bot> botRepository = botRepository;
        private readonly IMapper mapper = mapper;

        public async Task<ListResponse<BotDto>> GetAll()
        {
            var bots = await botRepository.GetAll();
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

        public async Task<BotDto> Get(int botId)
        {
            var bot = await botRepository.GetById(botId) ?? throw new EmptyItemException();
            return mapper.Map<Bot, BotDto>(bot);
        }
    }
}
