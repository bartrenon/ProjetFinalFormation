using BLL.Dtos.Set;
using BLL.Interfaces;
using BLL.Mappers;
using DAL.Interfaces;
using Domain.Entities;

namespace BLL.Services;

public class SetService : ISetService
{
    private readonly ISetRepository _setRepository;
    private readonly ICardRepository _cardRepository;

    public SetService(ISetRepository setRepository, ICardRepository cardRepository)
    {
        _setRepository = setRepository;
        _cardRepository = cardRepository;
    }

    public async Task<IEnumerable<Set>> GetFilteredSetsAsync(int pageNumber, int pageSize, string? name)
    {
        int offset = (pageNumber - 1) * pageSize;

        return await _setRepository.GetFilteredSetsAsync(offset, pageSize, name);
    }

    public async Task<SetDetailDto?> GetByIdWithCardsAsync(string id)
    {
        Set? set = await _setRepository.GetByIdWithCardsAsync(id);

        if (string.IsNullOrWhiteSpace(id))
        {
            return null;
        }

        IEnumerable<Card> cards = await _cardRepository.GetBySetIdAsync(id);

        return  SetMapper.ToSetDetailDto(set!, cards);
    }
}
