using Domain.Entities;

namespace BLL.Interfaces;

public interface ICardService
{
    Task<IEnumerable<Card>> GetFilteredCardsAsync(int pageNumber, int pageSize, string? name);

    Task<Card?> GetByIdAsync(string id);

    Task<IEnumerable<Card>> GetBySetIdAsync(string setId);
}
