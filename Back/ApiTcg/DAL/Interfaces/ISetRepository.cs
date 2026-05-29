using Domain.Entities;

namespace DAL.Interfaces;

public interface ISetRepository
{
    Task<IEnumerable<Set>> GetAllAsync();
    Task<Set?> GetByIdAsync(string id);
    Task UpsertAsync(Set set);
}
