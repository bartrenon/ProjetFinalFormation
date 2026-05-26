using Domain.Entities;

namespace DAL.Interfaces;

public interface IUserRepository
{
    Task<int> RegisterAsync(User user);
    Task<User?> GetByEmailAsync(string email);
}
