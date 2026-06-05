using Domain.Entities;

namespace BLL.Interfaces;

public interface IUserCardService
{
    Task<UserCard?> GetByIdAsync(string id);

    Task<int> DeleteUserCardAsync(string id);

    Task<int> AddUserCardAsync(UserCard userCard);
}
