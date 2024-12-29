using MediatR;
using Microsoft.AspNetCore.Mvc;
using SteamTradeHelper.Dtos;
using SteamTradeHelper.Services.Querires;
using SteamTradeHelper.Utilities.Exceptions;

namespace SteamTradeHelper.API.Controllers
{
    [Route("api/bots")]
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class BotController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator mediator = mediator;

        [HttpGet("", Name = "GetAllBots")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ListResponse<BotDto>>> GetAllBots()
        {
            try
            {
                var response = await mediator.Send(new GetBotsQuery());
                return Ok(response);
            }
            catch (EmptyListException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{botId}", Name = "GetBot")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GameDto>> GetBot(
            [FromRoute] int botId)
        {
            try
            {
                var response = await mediator.Send(new GetBotQuery(botId));
                return Ok(response);
            }
            catch (EmptyItemException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
