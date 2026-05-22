using ApiTcg.DTO.User;
using Domain.Entities;

namespace ApiTcg.Mappers;

public class UserMapper
{
    public static User ToUser(UserCreate u) 
    {
        return new User
        {
            Username = u.Username,
            Email = u.Email,
            PasswordHash = u.PasswordHash
        };
    }
}
