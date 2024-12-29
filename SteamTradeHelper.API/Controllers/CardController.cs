using MediatR;
using Microsoft.AspNetCore.Mvc;
using SteamTradeHelper.Dtos;
using SteamTradeHelper.Services.Commands;
using SteamTradeHelper.Services.Querires;
using SteamTradeHelper.Utilities.Exceptions;

namespace SteamTradeHelper.API.Controllers
{
    [Route("api/games/{gameId}/cards")]
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class CardController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator mediator = mediator;

        [HttpGet("", Name = "GetAllCardsByGame")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ListResponse<CardDto>>> GetAllCardsByGame(
            [FromRoute] int gameId)
        {
            try
            {
                var response = await mediator.Send(new GetCardsByGameQuery(gameId));
                return Ok(response);
            }
            catch (EmptyListException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPatch("set-tradeability", Name = "SetCardsTradeabilityByGame")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> SetCardsTradeabilityByGame(
            [FromRoute] int gameId)
        {
            try
            {
                await mediator.Send(new SetCardsTradeabilityByGameCommand(gameId));
                return Ok();
            }
            catch (EmptyItemException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
