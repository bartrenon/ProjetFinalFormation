using BLL.Interfaces;
using DAL.Interfaces;
using Domain.Entities;

namespace BLL.Services;

public class CardPriceService : ICardPriceService
{
    private readonly ICardPriceRepository _cardPriceRepository;
    private readonly IImportService _importService;
    private static readonly TimeSpan timeExpired = TimeSpan.FromHours(24);

    public CardPriceService(ICardPriceRepository cardPriceRepository, IImportService importService)
    {
        _cardPriceRepository = cardPriceRepository;
        _importService = importService;
    }

    public async Task<CardPrice?> GetByCardIdAsync(string cardId)
    {
        CardPrice? price = await _cardPriceRepository.GetByCardIdAsync(cardId);

        bool isExpired = price is null || IsExpired(price.UpdatedAt);

        if (!isExpired) 
        {
            return price;
        }

        CardPrice? freshPrice = await FetchAndUpsertPriceAsync(cardId);

        return freshPrice ?? price; 
    }

    private async Task<CardPrice?> FetchAndUpsertPriceAsync(string cardId)
    {
        int val = await _importService.ImportPricesForCardAsync(cardId);

        if (val == 0)
        {
            return null;
        }

       return await _cardPriceRepository.GetByCardIdAsync(cardId);
    }

    private static bool IsExpired(DateTime updatedAt)
    {
        return DateTime.UtcNow - updatedAt > timeExpired;
    }

    public async Task<decimal> GetTotalCollectionValueAsync(int userId)
    {
        return await _cardPriceRepository.GetTotalCollectionValueAsync(userId);
    }

    public async Task<decimal> GetTotalValueBySetAsync(int userId, string setId)
    {
        return await _cardPriceRepository.GetTotalCollectionValueBySetAsync(userId, setId);
    }
}
