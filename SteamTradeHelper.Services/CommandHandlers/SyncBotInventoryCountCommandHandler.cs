using MediatR;
using SteamTradeHelper.Client.Contracts;
using SteamTradeHelper.Context.Models;
using SteamTradeHelper.Repositories.Contracts;
using SteamTradeHelper.Services.Commands;
using SteamTradeHelper.Utilities.Exceptions;

namespace SteamTradeHelper.Services.CommandHandlers
{
    public class SyncBotInventoryCountCommandHandler(IBaseRepository<Bot> botRepository, ISteamClient client) : IRequestHandler<SyncBotInventoryCountCommand>
    {
        private readonly IBaseRepository<Bot> botRepository = botRepository;
        private readonly ISteamClient client = client;

        public async Task Handle(SyncBotInventoryCountCommand request, CancellationToken cancellationToken)
        {
            var bot = await botRepository.GetById(request.BotId) ?? throw new EmptyItemException();
            var response = await client.GetSteamInventoryCountAsync(bot.SteamId);
            bot.InventoryCount = response.Result?.total_inventory_count;
            await botRepository.Put(bot);
        }
    }
}
