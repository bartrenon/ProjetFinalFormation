using Domain.Entities;

namespace DAL.Interfaces;

public interface ISetRepository
{
    Task<(IEnumerable<Set>, int)> GetFilteredSetsAsync(int offset, int pageSize, string? name);
    Task<Set?> GetByIdWithCardsAsync(string id);
    Task UpsertAsync(Set set);
}
