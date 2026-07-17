using BLL.Dtos.Collection;
using Domain.Entities;

namespace BLL.Mappers;

public class CollectionMapper
{
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
