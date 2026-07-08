using BLL.Dtos.Collection;
using Domain.Entities;

namespace BLL.Mappers;

public class CollectionMapper
{
    public static Collection ToCollection(CollectionAddDto c)
    {
        return new Collection
        {
            UserId = c.UserId,
            CardId = c.CardId
        };
    }

    public static CollectionSummaryDto ToCollectionSummary(Collection c)
    {
        return new CollectionSummaryDto
        {
            Id = c.Id,
            NbDuplicateCard = c.NbDuplicateCard,
            CreatedAt = c.CreatedAt
        };
    }
}
