using Domain.Entities;

namespace DAL.Interfaces;

public interface IUserRepository
{
    Task<int> RegisterAsync(User user);
    Task<User?> GetByEmailAsync(string email);
    Task<int> SoftDeleteUserAsync(int userId);
    Task<int> HardDeleteUserAsync(int userId);
    Task<int> HardDeleteUserAsync(DateTime? deletedDate);
}
