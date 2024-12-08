using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteamTradeHelper.Dtos;

namespace SteamTradeHelper.Services.Contracts
{
    public interface ICardService
    {
        Task<ListResponse<CardDto>> GetAll();

        Task<ListResponse<CardDto>> GetAll(int gameId);

        Task SetTradeability(int gameId);
    }
}
