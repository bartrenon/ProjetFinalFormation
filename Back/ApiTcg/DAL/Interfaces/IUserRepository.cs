using Domain.Entities;

namespace DAL.Interfaces;

public interface IUserRepository
{
    Task<int> RegisterAsync(User user);
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByEmailAsync(string email);
    Task<int> SoftDeleteUserAsync(int userId);
    Task<int> HardDeleteUserAsync(int userId);
    Task<int> HardDeleteUserAsync(DateTime? deletedDate);
    Task<int> IsEmailTakenAsync(string email);
    Task<int> IsUsernameTakenAsync(string username);
}
