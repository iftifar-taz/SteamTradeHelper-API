using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SteamTradeHelper.Client.Contracts;
using SteamTradeHelper.Client.Models.Games;
using SteamTradeHelper.Context.Models;
using SteamTradeHelper.Dtos;
using SteamTradeHelper.Repositories.Contracts;
using SteamTradeHelper.Services.Commands;

namespace SteamTradeHelper.Services.CommandHandlers
{
    public class SyncGamesCommandHandler(IBaseRepository<Game> gameRepository, IOptions<SteamSettings> options,
        IMapper mapper, ISteamClient client) : IRequestHandler<SyncGamesCommand>
    {
        private readonly IBaseRepository<Game> gameRepository = gameRepository;
        private readonly SteamSettings steamSettings = options.Value;
        private readonly IMapper mapper = mapper;
        private readonly ISteamClient client = client;

        public async Task Handle(SyncGamesCommand request, CancellationToken cancellationToken)
        {
            var existingGames = await gameRepository.GetAll();
            var existingGameAppIds = existingGames.Select(x => x.AppId);
            var response = await client.GetSteamGamesAsync(steamSettings.SteamId, steamSettings.SteamKey);
            var tempResult = mapper.Map<IEnumerable<GameInformation>, IEnumerable<Game>>(response.Result?.response?.games ?? []);
            var result = tempResult.Where(x => !existingGameAppIds.Contains(x.AppId));
            await gameRepository.SaveAll(result);
        }
    }
}
