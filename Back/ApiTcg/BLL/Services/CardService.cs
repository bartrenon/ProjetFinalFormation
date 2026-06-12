using BLL.Interfaces;
using DAL.Interfaces;
using Domain.Entities;

namespace BLL.Services;

public class CardService : ICardService
{
    private readonly ICardRepository _cardRepository;

    public CardService(ICardRepository cardRepository) 
    {
       _cardRepository = cardRepository;
    }

    public async Task<IEnumerable<Card>> GetFilteredCardsAsync(int pageNumber, int pageSize, string? name)
    {
        int offset = (pageNumber - 1) * pageSize;

        return await _cardRepository.GetFilteredCardsAsync(offset, pageSize, name);
    }

    public async Task<Card?> GetByIdAsync(string id)
    {
        return await _cardRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Card>> GetBySetIdAsync(string setId)
    {
        return await _cardRepository.GetBySetIdAsync(setId);
    }
}
