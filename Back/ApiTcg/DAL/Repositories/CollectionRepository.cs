using DAL.Interfaces;
using Dapper;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DAL.Repositories;

public class CollectionRepository : ICollectionRepository
{
    private readonly string _connectionString;

    public CollectionRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task<int> AddCollectionAsync(int userId, string cardId)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        string query = @"INSERT INTO Collection (UserId, CardId, NbDuplicateCard)
                         VALUES (@UserId, @CardId, 1)";

        return await connection.ExecuteAsync(query, new { UserId = userId, CardId = cardId });
    }

    public async Task<int> DeleteCollectionAsync(int id)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        string query = @"DELETE FROM Collection
                             WHERE Id = @Id";

        return await connection.ExecuteAsync(query, new { Id = id });
    }

    public async Task<Collection?> GetByIdAsync(int userId, string cardId)
    {

        using  SqlConnection connection = new SqlConnection(_connectionString);

        string query = @"SELECT Id, UserId, CardId, CreatedAt, NbDuplicateCard
                             FROM Collection
                             WHERE UserId = @UserId AND CardId = @CardId";

        return await connection.QueryFirstOrDefaultAsync<Collection>(query, new { UserId = userId, CardId = cardId });

    }

    public async Task<bool> ExistsInCollectionAsync(int userId, string cardId) 
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        string query = @"SELECT count(*)
                             FROM Collection
                             WHERE UserId = @UserId AND CardId = @CardId";

        int count =  await connection.ExecuteScalarAsync<int>(query, new { UserId = userId, CardId = cardId });

        return count > 0;
    }

    public async Task<int> UpdateCollectionAsync(int id, bool isAdding)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        string query = "";

        if (isAdding) 
        {
            query = @$"UPDATE Collection 
                       SET NbDuplicateCard += 1 
                       WHERE Id = @Id";
        }
        else 
        {
            query = @$"UPDATE Collection 
                       SET NbDuplicateCard -= 1 
                       WHERE Id = @Id AND NbDuplicateCard > 1";
        }

        return await connection.ExecuteAsync(query, new { Id = id });
    }
}
