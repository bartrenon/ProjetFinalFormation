using BLL.Dtos.Collection;
using BLL.Interfaces;
using BLL.Mappers;
using DAL.Interfaces;
using Domain.Entities;

namespace BLL.Services;

public class CollectionService : ICollectionService
{
    private readonly ICollectionRepository _collectionRepository;

    public CollectionService(ICollectionRepository collectionRepository)
    {
        this._collectionRepository = collectionRepository;
    }

    public async Task<int> AddCollectionAsync(CollectionAddDto c)
    {
        Collection collection = CollectionMapper.ToCollection(c);

        return await _collectionRepository.AddCollectionAsync(collection);
    }

    public async Task<int> DeleteCollectionAsync(int id)
    {
        return await _collectionRepository.DeleteCollectionAsync(id);
    }

    public async Task<Collection?> GetByIdAsync(int userId, string cardId)
    {
        return await _collectionRepository.GetByIdAsync(userId, cardId);
    }
}
