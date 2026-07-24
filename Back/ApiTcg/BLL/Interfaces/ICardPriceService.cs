using Domain.Entities;

namespace BLL.Interfaces;

public interface ICardPriceService
{
    Task<CardPrice?> GetByCardIdAsync(string cardId);
}
