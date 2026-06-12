using Domain.Entities;

namespace DAL.Interfaces;

public interface ISetRepository
{
    Task<IEnumerable<Set>> GetFilteredSetsAsync(int offset, int pageSize, string? name);
    //Task<Set?> GetByIdWithCardsAsync(string id);
    Task<Set?> GetByIdAsync(string id);
    Task UpsertAsync(Set set);
}
