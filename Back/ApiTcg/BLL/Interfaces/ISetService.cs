using Domain.Entities;

namespace BLL.Interfaces;

public interface ISetService
{
    Task<IEnumerable<Set>> GetFilteredSets(int pageNumber, int pageSize, string? name);
    Task<Set?> GetByIdAsync(string id);
}
