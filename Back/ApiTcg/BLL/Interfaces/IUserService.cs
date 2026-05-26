using Domain.Entities;

namespace BLL.Interfaces;

public interface IUserService
{
    Task<int> RegisterAsync(User user);
    Task<string?> LoginAsync(string email, string password);
    Task<int> SoftDeleteUserAsync(int userId);
    Task<int> HardDeleteUserAsync(int userId);
    Task<int> HardDeleteUserAsync(DateTime? deletedDate);
}
