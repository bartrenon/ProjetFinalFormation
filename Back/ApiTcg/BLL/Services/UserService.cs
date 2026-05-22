using BLL.Interfaces;
using DAL.Interfaces;
using Domain.Entities;

namespace BLL.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        this._userRepository = userRepository;
    }

    public async Task<int> CreateAsync(User user)
    {
       user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash, workFactor : 12);

       int result = await _userRepository.CreateAsync(user);

       return result;
    }
}
