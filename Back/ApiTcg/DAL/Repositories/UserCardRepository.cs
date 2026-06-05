using DAL.Interfaces;
using Dapper;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DAL.Repositories;

public class UserCardRepository : IUserCardRepository
{
    private readonly string _connectionString;

    public UserCardRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task<int> AddUserCardAsync(UserCard userCard)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        string query = @"INSERT INTO UserCard (UserId ,CardId)
                         VALUES (@UserId ,@CardId)";

        return await connection.ExecuteAsync(query, userCard);
    }

    public async Task<int> DeleteUserCardAsync(string id)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        string query = @"DELETE FROM UserCard
                             WHERE Id = @Id";

        return await connection.ExecuteAsync(query, new { Id = id });
    }

    public async Task<UserCard?> GetByIdAsync(string id)
    {

        using  SqlConnection connection = new SqlConnection(_connectionString);

        string query = @"SELECT Id, UserId, CardId, CreatedAt
                             FROM UserCard
                             WHERE Id = @Id";

        return await connection.QueryFirstOrDefaultAsync<UserCard>(query, new { Id = id });

    }
}
