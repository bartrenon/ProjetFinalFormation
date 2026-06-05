using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Repositories;
using Domain.Entities;

namespace BLL.Services;

public class CollectionService : ICollectionService
{
    private readonly ICollectionRepository _collectionRepository;

    public CollectionService(ICollectionRepository collectionRepository)
    {
        this._collectionRepository = collectionRepository;
    }

    public async Task<int> AddCollectionAsync(Collection collection)
    {
        return await _collectionRepository.AddCollectionAsync(collection);
    }

    public async Task<int> DeleteCollectionAsync(string id)
    {
        return await _collectionRepository.DeleteCollectionAsync(id);
    }

    public async Task<Collection?> GetByIdAsync(string id)
    {
        return await _collectionRepository.GetByIdAsync(id);
    }
}
