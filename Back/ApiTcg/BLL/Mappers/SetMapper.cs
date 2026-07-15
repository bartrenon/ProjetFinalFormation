using BLL.Dtos.Card;
using BLL.Dtos.Set;
using DAL.Repositories;
using Domain.Entities;

namespace BLL.Mappers;
public class SetMapper
{
    public static SetDetailDto ToSetDetailDto(Set set, IEnumerable<CardSummaryDto> cards)
    {
        return new SetDetailDto
        {
            Id = set.Id,
            Name = set.Name,
            Logo = set.Logo,
            Symbol = set.Symbol,
            CardCountTotal = set.CardCountTotal,
            CardCountOfficial = set.CardCountOfficial,

            Cards = cards
        };
    }
}
