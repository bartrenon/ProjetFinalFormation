using Domain.Entities;

namespace DAL.Interfaces;
public interface ICollectionRepository
{
    Task<Collection?> GetByIdAsync(int userId, string cardId);
    Task<int> DeleteCollectionAsync(int id);
    Task<int> UpdateCollectionAsync(int id, bool isAdding);
    Task<int> AddCollectionAsync(int userId, string cardId);
    Task<bool> ExistsInCollectionAsync(int userId, string cardId);

}
