using BLL.Dtos.Collection;
using Domain.Entities;

namespace BLL.Interfaces;

public interface ICollectionService
{
    Task<CollectionSummaryDto?> GetByIdAsync(int userId, string cardId);
    Task<int> DeleteCollectionAsync(int id);
    Task<int> AddCollectionAsync(CollectionAddDto collection);
    Task<int> UpdateCollectionAsync(int id, bool isAdding);
}

