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

    public static CardDto ToCardDto(Card c)
    {
        return new CardDto
        {
            Id = c.Id,
            Name = c.Name,
            SetId = c.SetId,
            LocalId = c.LocalId,
            Image = c.Image,
            Set = SetMapper.ToSetSummaryDto(c.Set),
            Collection = c.Collections.FirstOrDefault() != null ? CollectionMapper.ToCollectionSummary(c.Collections.FirstOrDefault()!) : null
        };
    }
}
