using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SteamTradeHelper.Client.Contracts;
using SteamTradeHelper.Client.Models.Bots;
using SteamTradeHelper.Context.Models;
using SteamTradeHelper.Repositories.Contracts;
using SteamTradeHelper.Services.Commands;
using SteamTradeHelper.Utilities;

namespace SteamTradeHelper.Services.CommandHandlers
{
    public class SyncBotsCommandHandler(IBaseRepository<Bot> botRepository, ISteamClient client, IMapper mapper) : IRequestHandler<SyncBotsCommand>
    {
        private readonly IBaseRepository<Bot> botRepository = botRepository;
        private readonly ISteamClient client = client;
        private readonly IMapper mapper = mapper;

        public async Task Handle(SyncBotsCommand request, CancellationToken cancellationToken)
        {
            var existingBots = await botRepository.GetAll();
            var existingBotSteamIds = existingBots.Select(x => x.SteamId);
            var botsHtmlPage = await client.GetBotsPageAsync();
            var botsClientResponse = HtmlPerser.GetBots(botsHtmlPage);
            var botsClient = botsClientResponse.Where(x => !existingBotSteamIds.Contains(x.SteamId));

            // TODO: fix not found logo urls(img/default.jpg) with steam api call https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v2/?key=0B71FB46568A596BDA0D7DD520914701&steamids=1,2,3,4,5
            var bots = mapper.Map<IEnumerable<AsfBotResponse>, IEnumerable<Bot>>(botsClient);
            await botRepository.SaveAll(bots);
        }
    }
}
