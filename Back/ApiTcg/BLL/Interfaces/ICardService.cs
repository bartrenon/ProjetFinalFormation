using Domain.Entities;

namespace BLL.Interfaces;

public interface ICardService
{
    Task<IEnumerable<Card>> GetAllAsync();

    Task<Card?> GetByIdAsync(string id);

    Task<IEnumerable<Card>> GetBySetIdAsync(string setId);
}
