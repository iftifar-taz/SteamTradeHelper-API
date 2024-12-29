using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using SteamTradeHelper.Client.Contracts;
using SteamTradeHelper.Client.Models.Cards;
using SteamTradeHelper.Context.Models;
using SteamTradeHelper.Dtos;
using SteamTradeHelper.Repositories.Contracts;
using SteamTradeHelper.Services.Commands;
using SteamTradeHelper.Utilities;
using SteamTradeHelper.Utilities.Exceptions;

namespace SteamTradeHelper.Services.CommandHandlers
{
    public class SyncGameCardsCommandHandler(IUnitOfWork unitOfWork, IOptions<SteamSettings> options, IMapper mapper, ISteamClient client) : IRequestHandler<SyncGameCardsCommand>
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        private readonly SteamSettings steamSettings = options.Value;
        private readonly IMapper mapper = mapper;
        private readonly ISteamClient client = client;

        public async Task Handle(SyncGameCardsCommand request, CancellationToken cancellationToken)
        {
            var game = await unitOfWork.GameRepository.GetById(request.GameId) ?? throw new EmptyItemException();
            var cardQuery = unitOfWork.CardRepository.GetQueryable();
            cardQuery = cardQuery.Where(x => x.GameId == game.Id);
            var existingCards = await unitOfWork.CardRepository.GetAllQuery(cardQuery);

            if (existingCards.Any())
            {
                return;
            }

            var cardsHtmlPage = await client.GetSteamGameCardsPageAsync(game.AppId);
            var cardsInformation = HtmlPerser.GetGameCards(cardsHtmlPage);
            foreach (var cardInformation in cardsInformation)
            {
                var realCardNameHtmlPage = await client.GetSteamCardPageAsync(steamSettings.SteamAppId, game.AppId, cardInformation.Name ?? string.Empty);
                var cardName = HtmlPerser.GetCardName(realCardNameHtmlPage);
                if (cardName.StartsWith(game.AppId.ToString()))
                {
                    cardInformation.Name = $"{cardInformation.Name} (Trading Card)";
                }

                var cardHtmlPage = await client.GetSteamCardPageAsync(steamSettings.SteamAppId, game.AppId, cardInformation.Name ?? string.Empty);
                cardInformation.ItemId = HtmlPerser.GetItemId(cardHtmlPage);
                cardInformation.GameId = game.Id;
            }

            var cards = mapper.Map<IEnumerable<CardsResponse>, IEnumerable<Card>>(cardsInformation);

            if (cards.Any())
            {
                await unitOfWork.CardRepository.SaveAll(cards);
                game.UpdatedAt = DateTime.UtcNow;
                unitOfWork.GameRepository.Put(game);
            }
            await unitOfWork.SaveChangesAsync();
        }
    }
}
