using Domain.Entities;

namespace DAL.Interfaces;

public interface ICardRepository
{
    Task<IEnumerable<Card>> GetAllAsync();
    Task<Card?> GetByIdAsync(string id);
    Task<IEnumerable<Card>> GetBySetIdAsync(string setId);
    Task UpsertAsync(Card card);
}
