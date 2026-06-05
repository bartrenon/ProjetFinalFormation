using Domain.Entities;

namespace DAL.Interfaces;
public interface ICollectionRepository
{
    Task<Collection?> GetByIdAsync(string id);

    Task<int> DeleteCollectionAsync(string id);

    Task<int> AddCollectionAsync(Collection collection);
}
