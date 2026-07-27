using Domain.Entities;

namespace DAL.Interfaces;
public interface ICardPriceRepository
{
    Task UpsertAsync(CardPrice price);
    Task<CardPrice?> GetByCardIdAsync(string cardId);
    Task<decimal> GetTotalCollectionValueAsync(int userId);
    Task<decimal> GetTotalCollectionValueBySetAsync(int userId, string setId);
}
