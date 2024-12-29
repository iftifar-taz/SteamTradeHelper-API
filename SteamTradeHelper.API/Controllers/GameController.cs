using MediatR;
using Microsoft.AspNetCore.Mvc;
using SteamTradeHelper.Dtos;
using SteamTradeHelper.Services.Commands;
using SteamTradeHelper.Services.Querires;
using SteamTradeHelper.Utilities.Exceptions;

namespace SteamTradeHelper.API.Controllers
{
    [Route("api/games")]
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class GameController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator mediator = mediator;

        [HttpGet("", Name = "GetAllGames")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ListResponse<GameDto>>> GetAllGames()
        {
            try
            {
                var response = await mediator.Send(new GetGamesQuery());
                return Ok(response);
            }
            catch (EmptyListException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{gameId}", Name = "GetGame")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GameDto>> GetGame(
            [FromRoute] int gameId)
        {
            try
            {
                var response = await mediator.Send(new GetGameQuery(gameId));
                return Ok(response);
            }
            catch (EmptyItemException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPatch("set-tradeability", Name = "SetGamesTradeability")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetGamesTradeability()
        {
            try
            {
                await mediator.Send(new SetGamesTradeabilityCommand());
                return Ok();
            }
            catch (EmptyItemException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{gameId}/set-tradeability", Name = "SetGameTradeability")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetGameTradeability(
            [FromRoute] int gameId)
        {
            try
            {
                await mediator.Send(new SetGameTradeabilityCommand(gameId));
                return Ok();
            }
            catch (EmptyItemException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
