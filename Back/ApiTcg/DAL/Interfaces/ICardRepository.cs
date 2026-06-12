using Domain.Entities;

namespace DAL.Interfaces;

public interface ICardRepository
{
    Task<IEnumerable<Card>> GetFilteredCardsAsync(int offset, int pageSize, string? name);
    Task<Card?> GetByIdAsync(string id);
    Task<IEnumerable<Card>> GetBySetIdAsync(string setId);
    Task UpsertAsync(Card card);
}
