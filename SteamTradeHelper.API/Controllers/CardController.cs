using Microsoft.AspNetCore.Mvc;
using SteamTradeHelper.Dtos;
using SteamTradeHelper.Services.Contracts;
using SteamTradeHelper.Utilities.Exceptions;

namespace SteamTradeHelper.API.Controllers
{
    [Route("api/games/{gameId}/cards")]
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class CardController : ControllerBase
    {
        private readonly ICardService cardService;

        public CardController(ICardService cardService)
        {
            this.cardService = cardService;
        }

        [HttpGet("", Name = "GetAllCardsByGameId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ListResponse<CardDto>>> GetAllCardsByGameId(
            [FromRoute] int gameId)
        {
            try
            {
                var response = await this.cardService.GetAll(gameId);
                return this.Ok(response);
            }
            catch (EmptyListException e)
            {
                return this.BadRequest(e.Message);
            }
        }

        [HttpGet("set-tradeability", Name = "SetCardsTradeability")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetCardsTradeability(
            [FromRoute] int gameId)
        {
            try
            {
                await this.cardService.SetTradeability(gameId);
                return this.Ok();
            }
            catch (EmptyItemException e)
            {
                return this.BadRequest(e.Message);
            }
        }
    }
}
