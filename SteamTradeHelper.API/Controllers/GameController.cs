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
    public class GameController : ControllerBase
    {
        private readonly IGameService gameService;

        public GameController(IGameService gameService)
        {
            this.gameService = gameService;
        }

        [HttpGet("", Name = "GetAllGames")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ListResponse<GameDto>>> GetAllGames()
        {
            try
            {
                var response = await this.gameService.GetAll();
                return this.Ok(response);
            }
            catch (EmptyListException e)
            {
                return this.BadRequest(e.Message);
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
                var response = await this.gameService.Get(gameId);
                return this.Ok(response);
            }
            catch (EmptyItemException e)
            {
                return this.BadRequest(e.Message);
            }
        }

        [HttpGet("set-tradeability", Name = "SetGamesTradeability")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetGamesTradeability()
        {
            try
            {
                await this.gameService.SetTradeability();
                return this.Ok();
            }
            catch (EmptyItemException e)
            {
                return this.BadRequest(e.Message);
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
                await this.gameService.SetTradeability(gameId);
                return this.Ok();
            }
            catch (EmptyItemException e)
            {
                return this.BadRequest(e.Message);
            }
        }
    }
}
