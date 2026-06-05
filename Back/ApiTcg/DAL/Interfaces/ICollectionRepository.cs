using Domain.Entities;

namespace DAL.Interfaces;
public interface ICollectionRepository
{
    Task<Collection?> GetByIdAsync(int id);

    Task<int> DeleteCollectionAsync(int id);

    Task<int> AddCollectionAsync(Collection collection);
}
