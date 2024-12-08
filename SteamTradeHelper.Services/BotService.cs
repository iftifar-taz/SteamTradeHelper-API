using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SteamTradeHelper.Context.Models;
using SteamTradeHelper.Dtos;
using SteamTradeHelper.Repositories.Contracts;
using SteamTradeHelper.Services.Contracts;
using SteamTradeHelper.Utilities.Exceptions;

namespace SteamTradeHelper.Services
{
    public class BotService : IBotService
    {
        private readonly IBaseRepository<Bot> botRepository;
        private readonly IMapper mapper;

        public BotService(IBaseRepository<Bot> botRepository, IMapper mapper)
        {
            this.botRepository = botRepository;
            this.mapper = mapper;
        }

        public async Task<ListResponse<BotDto>> GetAll()
        {
            var bots = await this.botRepository.GetAll();
            if (!bots.Any())
            {
                throw new EmptyListException();
            }

            return new ListResponse<BotDto>()
            {
                List = this.mapper.Map<IEnumerable<Bot>, IReadOnlyCollection<BotDto>>(bots),
                Total = bots.Count(),
                LastSynced = bots.Max(x => x.UpdatedAt),
            };
        }

        public async Task<BotDto> Get(int botId)
        {
            var bot = await this.botRepository.GetById(botId);
            return bot == null ? throw new EmptyItemException() : this.mapper.Map<Bot, BotDto>(bot);
        }
    }
}
