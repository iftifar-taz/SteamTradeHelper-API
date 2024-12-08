using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteamTradeHelper.Dtos;

namespace SteamTradeHelper.Services.Contracts
{
    public interface IGameService
    {
        Task<ListResponse<GameDto>> GetAll();

        Task<GameDto> Get(int gameId);

        Task SetTradeability();

        Task SetTradeability(int gameId);
    }
}
