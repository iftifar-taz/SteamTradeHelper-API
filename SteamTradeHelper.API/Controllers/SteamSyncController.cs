using MediatR;
using Microsoft.AspNetCore.Mvc;
using SteamTradeHelper.Dtos;
using SteamTradeHelper.Services.Commands;

namespace SteamTradeHelper.API.Controllers
{
    [Route("api/steam-sync")]
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class SteamSyncController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator mediator = mediator;

        [HttpPost("games", Name = "SyncGames")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> SyncGames()
        {
            await mediator.Send(new SyncGamesCommand());
            return Ok();
        }

        [HttpPost("games/{gameId}/cards", Name = "SyncGameCards")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> SyncGameCards(
            [FromRoute] int gameId)
        {
            await mediator.Send(new SyncGameCardsCommand(gameId));
            return Ok();
        }

        [HttpPatch("games/{gameId}/cards/price", Name = "SyncGameCardPrices")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> SyncGameCardPrices(
            [FromRoute] int gameId)
        {
            await mediator.Send(new SyncGameCardPricesCommand(gameId));
            return Ok();
        }

        [HttpPost("bots", Name = "SyncBots")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> SyncBots()
        {
            await mediator.Send(new SyncBotsCommand());
            return Ok();
        }

        [HttpPatch("bots/{botId}/inventory-count", Name = "SyncBotInventoryCount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BotDto>> SyncBotInventoryCount(
            [FromRoute] int botId)
        {
            await mediator.Send(new SyncBotInventoryCountCommand(botId));
            return Ok();
        }
    }
}
