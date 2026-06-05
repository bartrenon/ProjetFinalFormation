using Domain.Entities;

namespace BLL.Interfaces;

public interface ICollectionService
{
    Task<Collection?> GetByIdAsync(int id);

    Task<int> DeleteCollectionAsync(int id);

    Task<int> AddCollectionAsync(Collection collection);
}
