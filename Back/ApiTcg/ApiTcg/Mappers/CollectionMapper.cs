using ApiTcg.Dtos.Collection;
using Domain.Entities;

namespace ApiTcg.Mappers;

public class CollectionMapper
{
    public static Collection ToCollection(CollectionAddDto uc)
    {
        return new Collection
        {
            UserId = uc.UserId,
            CardId = uc.CardId
        };
    }
}
