using BLL.Dtos.User;
using BLL.Interfaces;
using BLL.Mappers;
using DAL.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace BLL.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IConfiguration _configuration;

    public UserService(IConfiguration configuration, IUserRepository userRepository, IJwtService jwtService, IRefreshTokenRepository refreshTokenRepository)
    {

        this._userRepository = userRepository;
        this._jwtService = jwtService;
        this._refreshTokenRepository = refreshTokenRepository;
        _configuration = configuration;
    }

    public async Task<int> RegisterAsync(UserCreateDto user)
    {
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password, workFactor : 12);

        if (await IsEmailTakenAsync(user.Email)){
            throw new Exception("Email déjà utilisé.");
        }
        else if(await IsUsernameTakenAsync(user.Username)){
            throw new Exception("Pseudo déjà pris.");
        }

        User NewUser = UserMapper.ToUser(user);

        return await _userRepository.RegisterAsync(NewUser);
    }

    public async Task<int> SoftDeleteUserAsync(int userId)
    {
        if(userId == 0) {
            return 0;
        }

        int result = await _userRepository.SoftDeleteUserAsync(userId);

        if (result == 1)
        {
            await _refreshTokenRepository.RevokeAllByUserIdAsync(userId);
        }

        return result;
    }

    public async Task<int> HardDeleteUserAsync(int userId)
    {
        if (userId == 0)
        {
            return 0;
        }

        await _refreshTokenRepository.DeleteAllByUserIdAsync(userId);

        return await _userRepository.HardDeleteUserAsync(userId);
    }

    public async Task<int> HardDeleteUserAsync(DateTime? deletedDate)
    {
        if (deletedDate > DateTime.UtcNow)
        {
            return 0;
        }

        await _refreshTokenRepository.DeleteAllByDeletedUsersAsync(deletedDate);

        return await _userRepository.HardDeleteUserAsync(deletedDate);
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

    public async Task<UserJwtDto?> LoginAsync(UserLoginDto userLogin)
    {
        User? user = await _userRepository.GetByEmailAsync(userLogin.Email);

        if (user is null || user.IsDeleted)
        {
            return null;
        }

        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(userLogin.Password, user.PasswordHash);

        if (!isPasswordValid)
        {
            return null;
        }

        return await GenerateAuthResponseAsync(user);
    }

    public async Task<UserJwtDto?> RefreshTokenAsync(string refreshToken)
    {
        RefreshToken? storedToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);

        if (storedToken is null || !storedToken.IsActive)
        {
            return null;
        }

        await _refreshTokenRepository.RevokeAsync(storedToken);

        return await GenerateAuthResponseAsync(storedToken.User);
    }

    public async Task<bool> RevokeRefreshTokenAsync(string refreshToken)
    {
        RefreshToken? storedToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);

        if (storedToken is null || !storedToken.IsActive)
        {
            return false;
        }

        await _refreshTokenRepository.RevokeAsync(storedToken);
        return true;
    }

    private async Task<UserJwtDto> GenerateAuthResponseAsync(User user)
    {
        string accessToken = _jwtService.GenerateAccessToken(user);
        string refreshTokenValue = _jwtService.GenerateRefreshToken();

        int refreshTokenExpirationDays = int.Parse(_configuration["Jwt:RefreshTokenExpirationDays"]!);

        RefreshToken refreshToken = new RefreshToken
        {
            Token = refreshTokenValue,
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(refreshTokenExpirationDays)
        };

        await _refreshTokenRepository.AddAsync(refreshToken);

        return new UserJwtDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshTokenValue
        };
    }
}
