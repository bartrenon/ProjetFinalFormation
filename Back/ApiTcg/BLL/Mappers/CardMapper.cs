using BLL.Dtos.Card;
using BLL.Dtos.Set;
using Domain.Entities;

namespace BLL.Mappers;
public class CardMapper
{
    public static CardSummaryDTO ToCardSummaryDTO(Card c)
    {
        return new CardSummaryDTO
        {
            Id = c.Id,
            Name = c.Name,
            LocalId = c.LocalId,
            Image = c.Image
        };
    }
}
