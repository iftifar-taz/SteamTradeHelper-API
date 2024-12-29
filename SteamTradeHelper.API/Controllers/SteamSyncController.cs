using Microsoft.AspNetCore.Mvc;
using SteamTradeHelper.Dtos;
using SteamTradeHelper.Services.Contracts;

namespace SteamTradeHelper.API.Controllers
{
    [Route("api/steam-sync")]
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class SteamSyncController(ISteamSyncService steamGamesService) : ControllerBase
    {
        private readonly ISteamSyncService steamGamesService = steamGamesService;

        [HttpPost("games", Name = "SyncGames")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> SyncGames()
        {
            await steamGamesService.SyncGames();
            return Ok();
        }

        [HttpPost("games/{gameId}/cards", Name = "SyncGameCards")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> SyncGameCards(
            [FromRoute] int gameId)
        {
            await steamGamesService.SyncGameCards(gameId);
            return Ok();
        }

        [HttpPatch("games/{gameId}/cards/price", Name = "SyncGameCardPrices")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> SyncGameCardPrices(
            [FromRoute] int gameId)
        {
            await steamGamesService.SyncCardPrices(gameId);
            return Ok();
        }

        [HttpPost("bots", Name = "SyncBots")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> SyncBots()
        {
            await steamGamesService.SyncBots();
            return Ok();
        }

        [HttpPatch("bots/{botId}/inventory-count", Name = "SyncBotInventoryCount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BotDto>> SyncBotInventoryCount(
            [FromRoute] int botId)
        {
            await steamGamesService.SyncBotInventoryCount(botId);
            return Ok();
        }
    }
}
