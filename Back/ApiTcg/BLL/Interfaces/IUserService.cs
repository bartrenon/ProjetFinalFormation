using BLL.Dtos.User;

namespace BLL.Interfaces;

public interface IUserService
{
    Task<int> RegisterAsync(UserCreateDto user);
    Task<string?> LoginAsync(UserLoginDto userLogin);
    Task<int> SoftDeleteUserAsync(int userId);
    Task<int> HardDeleteUserAsync(int userId);
    Task<int> HardDeleteUserAsync(DateTime? deletedDate);
    Task<bool> IsEmailTakenAsync(string email);
    Task<bool> IsUsernameTakenAsync(string username);
}
