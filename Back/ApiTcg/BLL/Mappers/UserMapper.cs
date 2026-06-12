using BLL.Dtos.User;
using Domain.Entities;

namespace BLL.Mappers;

public class UserMapper
{
    public static User ToUser(UserCreateDto u) 
    {
        return new User
        {
            Username = u.Username,
            Email = u.Email,
            PasswordHash = u.Password
        };
    }
}
