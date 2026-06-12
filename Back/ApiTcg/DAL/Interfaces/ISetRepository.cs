using Domain.Entities;

namespace DAL.Interfaces;

public interface ISetRepository
{
    Task<IEnumerable<Set>> GetFilteredSets(int offset, int pageSize, string? name);
    Task<Set?> GetByIdAsync(string id);
    Task UpsertAsync(Set set);
}
