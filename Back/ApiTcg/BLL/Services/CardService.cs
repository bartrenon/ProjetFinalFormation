using BLL.Interfaces;
using DAL.Interfaces;
using Domain.Entities;

namespace BLL.Services;

public class CardService : ICardService
{
    private readonly ICardRepository _cardRepository;
    private readonly ICollectionRepository _collectionRepository;

    public CardService(ICardRepository cardRepository, ICollectionRepository collectionRepository) 
    {
       _cardRepository = cardRepository;
       _collectionRepository = collectionRepository;
    }

    public async Task<IEnumerable<Card>> GetFilteredCardsAsync(int pageNumber, int pageSize, string? name)
    {
        int offset = (pageNumber - 1) * pageSize;

        IEnumerable<Card> cards = await _cardRepository.GetFilteredCardsAsync(offset, pageSize, name);

        foreach (Card c in cards)
        {
            Collection? collection = await _collectionRepository.GetByIdAsync(1, c.Id);
            if (collection != null) 
            {
                c.Collections.Add(collection);
            }
        }

        return cards;
    }

    public async Task<Card?> GetByIdAsync(string id)
    {
        Card? card =  await _cardRepository.GetByIdAsync(id);

        if(card is null)
        {
            return null;
        }

        Collection ? collection = await _collectionRepository.GetByIdAsync(1, card.Id);
        if (collection != null)
        {
            card.Collections.Add(collection);
        }

        return card;
    }

    public async Task<IEnumerable<Card>> GetBySetIdAsync(string setId)
    {
        IEnumerable<Card> cards = await _cardRepository.GetBySetIdAsync(setId);

        foreach (Card c in cards)
        {
            Collection? collection = await _collectionRepository.GetByIdAsync(1, c.Id);
            if (collection != null)
            {
                c.Collections.Add(collection);
            }
        }

        return cards;
    }
}
