using BLL.Dtos.Card;
using BLL.Dtos.Set;
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

    public static SetSummaryDto ToSetSummaryDto(Set set)
    {
        return new SetSummaryDto
        {
            Name = set.Name,
            Symbol = set.Symbol
        };
    }

    public static SetDto ToSetDto(Set set, bool isCompleted)
    {
        return new SetDto
        {
            Id = set.Id,
            Name = set.Name,
            Logo = set.Logo,
            Symbol = set.Symbol,
            IsCompleted = isCompleted
        };
    }

    public static SetWithPaginationDto ToSetWithPaginationDto(IEnumerable<Set> sets, int nbSet, IEnumerable<bool> completed)
    {
        return new SetWithPaginationDto
        {
            sets = sets
             .Zip(completed, (set, isCompleted) => ToSetDto(set, isCompleted))
             .ToList(),
            totalSets = nbSet
        };
    }
}
