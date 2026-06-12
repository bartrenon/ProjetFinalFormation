using BLL.Interfaces;
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

    public async Task<Set?> GetByIdWithCardsAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return null;
        }

        return await _setRepository.GetByIdAsync(id);

        //return await _setRepository.GetByIdWithCardsAsync(id);
    }
}
