using MediatR;
using SteamTradeHelper.Client.Contracts;
using SteamTradeHelper.Repositories.Contracts;
using SteamTradeHelper.Services.Commands;
using SteamTradeHelper.Utilities.Exceptions;

namespace SteamTradeHelper.Services.CommandHandlers
{
    public class SyncBotInventoryCountCommandHandler(IUnitOfWork unitOfWork, ISteamClient client) : IRequestHandler<SyncBotInventoryCountCommand>
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        private readonly ISteamClient client = client;

        public async Task Handle(SyncBotInventoryCountCommand request, CancellationToken cancellationToken)
        {
            var bot = await unitOfWork.BotRepository.GetById(request.BotId) ?? throw new EmptyItemException();
            var response = await client.GetSteamInventoryCountAsync(bot.SteamId);
            bot.InventoryCount = response.Result?.total_inventory_count;
            unitOfWork.BotRepository.Put(bot);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
