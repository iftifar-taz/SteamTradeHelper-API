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
    public class SyncGamesCommandHandler(IUnitOfWork unitOfWork, IOptions<SteamSettings> options,
        IMapper mapper, ISteamClient client) : IRequestHandler<SyncGamesCommand>
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        private readonly SteamSettings steamSettings = options.Value;
        private readonly IMapper mapper = mapper;
        private readonly ISteamClient client = client;

        public async Task Handle(SyncGamesCommand request, CancellationToken cancellationToken)
        {
            var existingGames = await unitOfWork.GameRepository.GetAll();
            var existingGameAppIds = existingGames.Select(x => x.AppId);
            var response = await client.GetSteamGamesAsync(steamSettings.SteamId, steamSettings.SteamKey);
            var tempResult = mapper.Map<IEnumerable<GameInformation>, IEnumerable<Game>>(response.Result?.response?.games ?? []);
            var result = tempResult.Where(x => !existingGameAppIds.Contains(x.AppId));
            await unitOfWork.GameRepository.SaveAll(result);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
