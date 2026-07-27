using Domain.Entities;

namespace BLL.Interfaces;

public interface ICardPriceService
{
    Task<CardPrice?> GetByCardIdAsync(string cardId);
    Task<decimal> GetTotalCollectionValueAsync(int userId);
    Task<decimal> GetTotalValueBySetAsync(int userId, string setId);
}
