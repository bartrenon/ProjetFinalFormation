using Domain.Entities;

namespace DAL.Interfaces;
public interface ICollectionRepository
{
    Task<Collection?> GetByIdAsync(int userId, string cardId);

    Task<int> DeleteCollectionAsync(int id);

    Task<int> AddCollectionAsync(Collection collection);
}
