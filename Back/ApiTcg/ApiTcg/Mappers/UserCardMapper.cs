using ApiTcg.Dtos.UserCard;
using Domain.Entities;

namespace ApiTcg.Mappers;

public class UserCardMapper
{
    public static UserCard ToUserCard(UserCardCreateDto uc)
    {
        return new UserCard
        {
            UserId = uc.UserId,
            CardId = uc.CardId
        };
    }
}
