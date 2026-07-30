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

    public async Task<int> AddCollectionAsync(int userId, string cardId)
    {
        bool isExist = await _collectionRepository.ExistsInCollectionAsync(userId, cardId);

        if(isExist){
            return 0;
        }

        return await _collectionRepository.AddCollectionAsync(userId, cardId);
    }

    public async Task<bool> CollectionSetIsCompletedAsync(int userId, string setId)
    {
        return await _collectionRepository.CollectionSetIsCompletedAsync(userId, setId);
    }

    public async Task<int> DeleteCollectionAsync(int id)
    {
        return await _collectionRepository.DeleteCollectionAsync(id);
    }

    public async Task<CollectionSummaryDto?> GetByIdAsync(int userId, string cardId)
    {
        Collection? c =  await _collectionRepository.GetByIdAsync(userId, cardId);

        if(c is not null){
            return CollectionMapper.ToCollectionSummary(c);
        }

        return null;
    }

    public async Task<int> UpdateCollectionAsync(int id, bool isAdding)
    {
        return await  _collectionRepository.UpdateCollectionAsync(id, isAdding);
    }

    public async Task<IEnumerable<CollectionCardDto>> GetAllByUserAsync(int userId)
    {
        var collections = await _collectionRepository.GetAllByUserAsync(userId);
        return collections.Select(CollectionMapper.ToCollectionCardDto);
    }
}
