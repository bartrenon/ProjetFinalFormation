using Domain.Entities;

namespace BLL.Interfaces;

public interface ISetService
{
    Task<IEnumerable<Set>> GetAllAsync();
    Task<Set?> GetByIdAsync(string id);
}
