using BLL.Dtos.Card;
using BLL.Dtos.Set;
using BLL.Interfaces;
using BLL.Mappers;
using DAL.Interfaces;
using DAL.Repositories;
using Domain.Entities;

namespace BLL.Services;

public class SetService : ISetService
{
    private readonly ISetRepository _setRepository;
    private readonly ICardService _cardService;
    private readonly ICollectionService _collectionService;
    public SetService(ISetRepository setRepository, ICardService cardService, ICollectionService collectionService)
    {
        _setRepository = setRepository;
        _cardService = cardService;
        _collectionService = collectionService;
    }

    public async Task<SetWithPaginationDto> GetFilteredSetsAsync(int userId, int pageNumber, int pageSize, string? name)
    {

        int offset = (pageNumber - 1) * pageSize;

        List<bool> filter = new List<bool>();

        (IEnumerable<Set> sets, int nbSet) = await _setRepository.GetFilteredSetsAsync(offset, pageSize, name);

        foreach (Set set in sets)
        {
            filter.Add(await _collectionService.CollectionSetIsCompletedAsync(userId, set.Id));
        }

        return SetMapper.ToSetWithPaginationDto(sets, nbSet, filter);
    }

    public async Task<SetDetailDto?> GetByIdWithCardsAsync(string id, int userId)
    {
        Set? set = await _setRepository.GetByIdWithCardsAsync(id);

        if (string.IsNullOrWhiteSpace(id)){
            return null;
        }

        IEnumerable<CardSummaryDto> cards = await _cardService.GetBySetIdAsync(id, userId);

        return  SetMapper.ToSetDetailDto(set!, cards);
    }

}
