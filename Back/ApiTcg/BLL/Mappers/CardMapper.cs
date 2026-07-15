using BLL.Dtos.Card;
using Domain.Entities;

namespace BLL.Mappers;
public class CardMapper
{
    public static CardSummaryDto ToCardSummaryDto(Card c, bool isInCollection)
    {
        return new CardSummaryDto
        {
            Id = c.Id,
            Name = c.Name,
            LocalId = c.LocalId,
            Image = c.Image,
            IsInCollection = isInCollection
        };
    }
}
