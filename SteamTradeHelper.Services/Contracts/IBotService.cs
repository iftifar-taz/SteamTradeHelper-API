using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteamTradeHelper.Dtos;

namespace SteamTradeHelper.Services.Contracts
{
    public interface IBotService
    {
        Task<ListResponse<BotDto>> GetAll();

        Task<BotDto> Get(int botId);
    }
}
