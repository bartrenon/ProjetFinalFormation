using BLL.Interfaces;

using DAL.Interfaces;
using Domain.Entities;

namespace BLL.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public UserService(IUserRepository userRepository, IJwtService jwtService)
    {
        this._userRepository = userRepository;
        this._jwtService = jwtService;
    }

    public async Task<int> RegisterAsync(User user)
    {
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash, workFactor : 12);

        if (await IsEmailTakenAsync(user.Email)) 
        {
            throw new Exception("Email déjà utilisé.");
        }
            

        if (await IsUsernameTakenAsync(user.Username)) 
        {
            throw new Exception("Pseudo déjà pris.");
        }

        int result = await _userRepository.RegisterAsync(user);

        return result;
    }

    public async Task<string?> LoginAsync(string email, string password)
    {
        User? user = await _userRepository.GetByEmailAsync(email);

        if (user is null)
        {
            return null;
        }

        if (user.IsDeleted) 
        {
            return null;
        }

        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

        if (!isPasswordValid)
        {
            return null;
        }

        return _jwtService.GenerateToken(user);
    }

    public async Task<int> SoftDeleteUserAsync(int userId)
    {
        int result = 0;

        if(userId == 0) 
        {
            return result;
        }
        
        result = await _userRepository.SoftDeleteUserAsync(userId);

        return result;
    }

    public async Task<int> HardDeleteUserAsync(int userId)
    {
        int result = 0;

        if (userId == 0)
        {
            return result;
        }

        result = await _userRepository.HardDeleteUserAsync(userId);

        return result;

    }

    public async Task<int> HardDeleteUserAsync(DateTime? deletedDate)
    {
        int result = 0;

        if ( deletedDate > DateTime.UtcNow)
        {
            return result;
        }

        result = await _userRepository.HardDeleteUserAsync(deletedDate);

        return result;
    }

    public async Task<bool> IsEmailTakenAsync(string email)
    {
        int result = await _userRepository.IsEmailTakenAsync(email);

        return result > 0;
    }

    public async Task<bool> IsUsernameTakenAsync(string username)
    {
        int result = await _userRepository.IsUsernameTakenAsync(username);

        return result > 0;
    }
}
