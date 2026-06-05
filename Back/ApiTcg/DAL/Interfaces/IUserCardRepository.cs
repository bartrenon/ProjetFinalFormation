using Domain.Entities;

namespace DAL.Interfaces;
public interface IUserCardRepository
{
    Task<UserCard?> GetByIdAsync(string id);

    Task<int> DeleteUserCardAsync(string id);

    Task<int> AddUserCardAsync(UserCard userCard);
}
