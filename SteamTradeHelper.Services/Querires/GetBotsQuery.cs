using MediatR;
using SteamTradeHelper.Dtos;

namespace SteamTradeHelper.Services.Querires
{
    public class GetBotsQuery : IRequest<ListResponse<BotDto>>
    {
    }
}
