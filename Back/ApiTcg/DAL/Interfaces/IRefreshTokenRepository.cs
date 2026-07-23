using Domain.Entities;

namespace DAL.Interfaces;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken refreshToken);
    Task<RefreshToken?> GetByTokenAsync(string token);
    Task RevokeAsync(RefreshToken refreshToken);
    Task RevokeAllByUserIdAsync(int userId);
    Task DeleteAllByUserIdAsync(int userId);
    Task DeleteAllByDeletedUsersAsync(DateTime? deletedDate);
}
