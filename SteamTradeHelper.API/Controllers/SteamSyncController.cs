using Microsoft.AspNetCore.Mvc;
using SteamTradeHelper.Dtos;
using SteamTradeHelper.Services.Contracts;

namespace SteamTradeHelper.API.Controllers
{
    [Route("api/steam-sync")]
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class SteamSyncController : ControllerBase
    {
        private readonly ISteamSyncService steamGamesService;

        public SteamSyncController(ISteamSyncService steamGamesService)
        {
            this.steamGamesService = steamGamesService;
        }

        [HttpGet("games", Name = "SyncGames")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> SyncGames()
        {
            await this.steamGamesService.SyncGames();
            return this.Ok();
        }

        [HttpGet("games/{gameId}/cards", Name = "SyncGameCards")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> SyncGameCards(
            [FromRoute] int gameId)
        {
            await this.steamGamesService.SyncGameCards(gameId);
            return this.Ok();
        }

        [HttpGet("games/{gameId}/card-prices", Name = "SyncGameCardPrices")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> SyncGameCardPrices(
            [FromRoute] int gameId)
        {
            await this.steamGamesService.SyncCardPrices(gameId);
            return this.Ok();
        }

        [HttpGet("bots", Name = "SyncBots")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> SyncBots()
        {
            await this.steamGamesService.SyncBots();
            return this.Ok();
        }

        [HttpGet("bots/{botId}/inventory-count", Name = "SyncBotInventoryCount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BotDto>> SyncBotInventoryCount(
            [FromRoute] int botId)
        {
            await this.steamGamesService.SyncBotInventoryCount(botId);
            return this.Ok();
        }
    }
}
