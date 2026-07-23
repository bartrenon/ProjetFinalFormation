using System.Data;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

using Dapper;

using DAL.Interfaces;

using Domain.Entities;

namespace DAL.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly string _connectionString;

    public RefreshTokenRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task AddAsync(RefreshToken refreshToken)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        const string query = @" INSERT INTO RefreshToken (Token, UserId, CreatedAt, ExpiresAt)
                                VALUES (@Token, @UserId, @CreatedAt, @ExpiresAt);";

        await connection.ExecuteAsync(query, refreshToken);
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        const string query = @" SELECT rt.Id, rt.Token, rt.UserId, rt.CreatedAt, rt.ExpiresAt, rt.RevokedAt,
                                        u.*
                                 FROM RefreshToken rt
                                 INNER JOIN [User] u ON u.Id = rt.UserId
                                 WHERE rt.Token = @Token;";

        IEnumerable<RefreshToken> result = await connection.QueryAsync<RefreshToken, User, RefreshToken>(
            query,
            (rt, user) =>
            {
                rt.User = user;
                return rt;
            },
            new { Token = token },
            splitOn: "Id");

        return result.FirstOrDefault();
    }

    public async Task RevokeAsync(RefreshToken refreshToken)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        const string query = @" UPDATE RefreshToken
                                SET RevokedAt = @RevokedAt
                                WHERE Id = @Id;";

        refreshToken.RevokedAt = DateTime.UtcNow;

        await connection.ExecuteAsync(query, new { refreshToken.RevokedAt, refreshToken.Id });
    }

    public async Task RevokeAllByUserIdAsync(int userId)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        const string query = @" UPDATE RefreshToken
                            SET RevokedAt = @RevokedAt
                            WHERE UserId = @UserId AND RevokedAt IS NULL;";

        await connection.ExecuteAsync(query, new { RevokedAt = DateTime.UtcNow, UserId = userId });
    }

    public async Task DeleteAllByUserIdAsync(int userId)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        const string query = "DELETE FROM RefreshToken WHERE UserId = @UserId;";

        await connection.ExecuteAsync(query, new { UserId = userId });
    }

    public async Task DeleteAllByDeletedUsersAsync(DateTime? deletedDate)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        string query = @" DELETE FROM RefreshToken
                      WHERE UserId IN (
                          SELECT Id FROM [User] WHERE (@DeletedAt IS NOT NULL AND DeletedAt <= @DeletedAt));";

        await connection.ExecuteAsync(query, new { DeletedAt = deletedDate });
    }
}