using Microsoft.AspNetCore.Mvc;
using SteamTradeHelper.Dtos;
using SteamTradeHelper.Services.Contracts;
using SteamTradeHelper.Utilities.Exceptions;

namespace SteamTradeHelper.API.Controllers
{
    [Route("api/games")]
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class GameController(IGameService gameService) : ControllerBase
    {
        private readonly IGameService gameService = gameService;

        [HttpGet("", Name = "GetAllGames")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ListResponse<GameDto>>> GetAllGames()
        {
            try
            {
                var response = await gameService.GetAll();
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
                var response = await gameService.Get(gameId);
                return Ok(response);
            }
            catch (EmptyItemException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("set-tradeability", Name = "SetGamesTradeability")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetGamesTradeability()
        {
            try
            {
                await gameService.SetTradeability();
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
                await gameService.SetTradeability(gameId);
                return Ok();
            }
            catch (EmptyItemException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
