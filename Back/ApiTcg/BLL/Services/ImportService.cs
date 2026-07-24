using System.Runtime.Intrinsics.X86;
using BLL.Interfaces;
using DAL.Interfaces;
using Domain.Entities;
using Infrastructure.Dtos.Card;
using Infrastructure.Dtos.Set;
using Infrastructure.External;

namespace BLL.Services;

public class ImportService : IImportService
{
    private readonly TcgDexClient _tcgDexClient;
    private readonly ISetRepository _setRepository;
    private readonly ICardRepository _cardRepository;
    private readonly ICardPriceRepository _cardPriceRepository;

    public ImportService(TcgDexClient tcgDexClient,
        ISetRepository setRepository, ICardRepository cardRepository, ICardPriceRepository cardPriceRepository)
    {
        _tcgDexClient = tcgDexClient;
        _setRepository = setRepository;
        _cardRepository = cardRepository;
        _cardPriceRepository = cardPriceRepository;
    }

    public async Task<int> ImportCardsAsync(string lang = "fr")
    {
        List<TcgDexCardBriefDto> cardsFromApi = await _tcgDexClient.GetAllCardsAsync(lang);
        int importedCount = 0;

        foreach (TcgDexCardBriefDto cardFromApi in cardsFromApi)
        {
            int lastDash = cardFromApi.Id.LastIndexOf('-');

            Card card = new()
            {
                Id = cardFromApi.Id,
                Name = cardFromApi.Name,
                Image = cardFromApi.Image,
                LocalId = cardFromApi.LocalId,
                SetId = cardFromApi.Id.Substring(0, lastDash)
            };

            await _cardRepository.UpsertAsync(card);
            importedCount++;
        }

        return importedCount;
    }

    public async Task<int> ImportSetsAsync(string lang = "fr")
    {
        List<TcgDexSetBriefDto> setsFromApi = await _tcgDexClient.GetAllSetsAsync(lang);
        int importedCount = 0;

        foreach (TcgDexSetBriefDto setFromApi in setsFromApi)
        {
            Set set = new()
            {
                Id = setFromApi.Id,
                Name = setFromApi.Name,
                Logo = setFromApi.Logo,
                Symbol = setFromApi.Symbol,
                CardCountTotal = setFromApi.CardCount.Total,
                CardCountOfficial = setFromApi.CardCount.Official,
            };

            await _setRepository.UpsertAsync(set);
            importedCount++;
        }

        return importedCount;
    }

    public async Task<int> ImportPricesForCardAsync(string cardId, string lang = "fr")
    {
        TcgDexCardDto? dto = await _tcgDexClient.GetCardAsync(cardId, lang);

        if (dto?.Pricing is null) return 0;

        decimal avg30 = dto.Pricing.Cardmarket?.Avg30 ?? 0;
        decimal avg = dto.Pricing.Cardmarket?.Avg ?? 0;

        if (avg30 == 0 && avg == 0) return 0;

        await _cardPriceRepository.UpsertAsync(new CardPrice
        {
            CardId = cardId,
            Avg30 = avg30,
            Avg = avg,
            UpdatedAt = DateTime.UtcNow
        });

        return 1;
    }
}
