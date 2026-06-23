using BLL.Dtos.Set;
using Domain.Entities;

namespace BLL.Mappers;
public class SetMapper
{
    public static SetDetailDto ToSetDetailDto(Set set, IEnumerable<Card> cards)
    {
        return new SetDetailDto
        {
            Id = set.Id,
            Name = set.Name,
            Logo = set.Logo,
            Symbol = set.Symbol,
            CardCountTotal = set.CardCountTotal,
            CardCountOfficial = set.CardCountOfficial,

            Cards = cards.Select(CardMapper.ToCardDto)
        };
    }
}
