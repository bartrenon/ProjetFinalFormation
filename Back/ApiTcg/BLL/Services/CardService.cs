using BLL.Dtos.Card;
using BLL.Interfaces;
using BLL.Mappers;
using DAL.Interfaces;
using Domain.Entities;

namespace BLL.Services;

public class CardService : ICardService
{
    private readonly ICardRepository _cardRepository;
    private readonly ICardPriceService _cardPriceService;
    private readonly ICollectionRepository _collectionRepository;

    public CardService(ICardRepository cardRepository, ICollectionRepository collectionRepository, ICardPriceService cardPriceService) 
    {
       _cardRepository = cardRepository;
       _collectionRepository = collectionRepository;
       _cardPriceService = cardPriceService;
    }

    public async Task<CardWithPaginationDto> GetFilteredCardsAsync(int pageNumber, int pageSize, string? name, int userId)
    {
        int offset = (pageNumber - 1) * pageSize;

        List<CardSummaryDto> cardsSummaryDto = new List<CardSummaryDto>();

        (IEnumerable<Card> cards, int nbCards) = await _cardRepository.GetFilteredCardsAsync(offset, pageSize, name);

        foreach (Card c in cards)
        {
            bool isExist = await _collectionRepository.ExistsInCollectionAsync(userId, c.Id);

            cardsSummaryDto.Add(CardMapper.ToCardSummaryDto(c, isExist));
        }

        return CardMapper.ToCardWithPaginationDto(cardsSummaryDto, nbCards);
    }

    public async Task<CardDto?> GetByIdAsync(string id, int userId)
    {
        Card? card =  await _cardRepository.GetByIdAsync(id);

        CardPrice? price = await _cardPriceService.GetByCardIdAsync(id);

        if(card is not null && card.Set is not null){
            Collection? collection = await _collectionRepository.GetByIdAsync(userId, card.Id);

            if (collection != null){
                card.Collections.Add(collection);
            }

            return CardMapper.ToCardDto(card, price);
        }

        return null;
    }

    public async Task<IEnumerable<CardSummaryDto>> GetBySetIdAsync(string setId, int userId)
    {
        List<CardSummaryDto> cardsSummaryDto = new List<CardSummaryDto>();

        IEnumerable<Card> cards = await _cardRepository.GetBySetIdAsync(setId);

        foreach (Card c in cards)
        {
            bool isExist = await _collectionRepository.ExistsInCollectionAsync(userId, c.Id);

            cardsSummaryDto.Add(CardMapper.ToCardSummaryDto(c, isExist));
        }

        return cardsSummaryDto;
    }
}
