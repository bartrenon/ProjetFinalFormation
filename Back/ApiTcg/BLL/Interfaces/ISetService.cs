using Domain.Entities;

namespace BLL.Interfaces;

public interface ISetService
{
    Task<IEnumerable<Set>> GetFilteredSetsAsync(int pageNumber, int pageSize, string? name);
    Task<Set?> GetByIdWithCardsAsync(string id);
}
