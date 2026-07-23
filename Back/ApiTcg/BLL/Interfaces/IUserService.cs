using BLL.Dtos.User;

namespace BLL.Interfaces;

public interface IUserService
{
    Task<int> RegisterAsync(UserCreateDto user);
    Task<UserJwtDto?> LoginAsync(UserLoginDto userLogin);
    Task<UserJwtDto?> RefreshTokenAsync(string refreshToken);
    Task<bool> RevokeRefreshTokenAsync(string refreshToken);
    Task<int> SoftDeleteUserAsync(int userId);
    Task<int> HardDeleteUserAsync(int userId);
    Task<int> HardDeleteUserAsync(DateTime? deletedDate);
    Task<bool> IsEmailTakenAsync(string email);
    Task<bool> IsUsernameTakenAsync(string username);
}
