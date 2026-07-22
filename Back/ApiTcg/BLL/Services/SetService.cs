using BLL.Dtos.Card;
using BLL.Dtos.Set;
using BLL.Interfaces;
using BLL.Mappers;
using DAL.Interfaces;
using Domain.Entities;

namespace BLL.Services;

public class SetService : ISetService
{
    private readonly ISetRepository _setRepository;
    private readonly ICardService _cardService;

    public SetService(ISetRepository setRepository, ICardService cardService)
    {
        _setRepository = setRepository;
        _cardService = cardService;
    }

    public async Task<SetWithPaginationDto> GetFilteredSetsAsync(int pageNumber, int pageSize, string? name)
    {
        int offset = (pageNumber - 1) * pageSize;

        (IEnumerable<Set> sets, int nbSet) = await _setRepository.GetFilteredSetsAsync(offset, pageSize, name);

        return SetMapper.ToSetWithPaginationDto(sets, nbSet);
    }

    public async Task<SetDetailDto?> GetByIdWithCardsAsync(string id, int userId)
    {
        Set? set = await _setRepository.GetByIdWithCardsAsync(id);

        if (string.IsNullOrWhiteSpace(id))
        {
            return null;
        }

        IEnumerable<CardSummaryDto> cards = await _cardService.GetBySetIdAsync(id, userId);

        return  SetMapper.ToSetDetailDto(set!, cards);
    }

}
