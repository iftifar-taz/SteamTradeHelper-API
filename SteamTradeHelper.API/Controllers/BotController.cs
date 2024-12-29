using Microsoft.AspNetCore.Mvc;
using SteamTradeHelper.Dtos;
using SteamTradeHelper.Services.Contracts;
using SteamTradeHelper.Utilities.Exceptions;

namespace SteamTradeHelper.API.Controllers
{
    [Route("api/bots")]
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class BotController(IBotService botService) : ControllerBase
    {
        private readonly IBotService botService = botService;

        [HttpGet("", Name = "GetAllBots")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ListResponse<BotDto>>> GetAllBots()
        {
            try
            {
                var response = await botService.GetAll();
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
                var response = await botService.Get(botId);
                return Ok(response);
            }
            catch (EmptyItemException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
