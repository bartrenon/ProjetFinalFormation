using BLL.Interfaces;
using DAL.Interfaces;
using Domain.Entities;

namespace BLL.Services;

public class CardPriceService : ICardPriceService
{
    private readonly ICardPriceRepository _cardPriceRepository;

    public CardPriceService(ICardPriceRepository cardPriceRepository)
    {
        _cardPriceRepository = cardPriceRepository;
    }

    public async Task<CardPrice?> GetByCardIdAsync(string cardId)
    {
        CardPrice? price = await _cardPriceRepository.GetByCardIdAsync(cardId);

        if (price is not null) 
        {
            return price;
        }

        return null;
    }
}
