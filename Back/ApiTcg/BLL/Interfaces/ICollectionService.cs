using Domain.Entities;

namespace BLL.Interfaces;

public interface ICollectionService
{
    Task<Collection?> GetByIdAsync(string id);

    Task<int> DeleteCollectionAsync(string id);

    Task<int> AddCollectionAsync(Collection collection);
}
