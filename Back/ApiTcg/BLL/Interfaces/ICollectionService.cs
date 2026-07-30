using BLL.Dtos.Collection;
using Domain.Entities;

namespace BLL.Interfaces;

public interface ICollectionService
{
    Task<CollectionSummaryDto?> GetByIdAsync(int userId, string cardId);
    Task<int> DeleteCollectionAsync(int id);
    Task<int> AddCollectionAsync(int userId, string cardId);
    Task<int> UpdateCollectionAsync(int id, bool isAdding);
    Task<bool> CollectionSetIsCompletedAsync(int userId, string setId);

    Task<IEnumerable<CollectionCardDto>> GetAllByUserAsync(int userId);
}

