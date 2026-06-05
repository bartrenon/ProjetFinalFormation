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

    public async Task<int> AddCollectionAsync(Collection collection)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        string query = @"INSERT INTO Collection (UserId ,CardId)
                         VALUES (@UserId ,@CardId)";

        return await connection.ExecuteAsync(query, collection);
    }

    public async Task<int> DeleteCollectionAsync(string id)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        string query = @"DELETE FROM Collection
                             WHERE Id = @Id";

        return await connection.ExecuteAsync(query, new { Id = id });
    }

    public async Task<Collection?> GetByIdAsync(string id)
    {

        using  SqlConnection connection = new SqlConnection(_connectionString);

        string query = @"SELECT Id, UserId, CardId, CreatedAt
                             FROM Collection
                             WHERE Id = @Id";

        return await connection.QueryFirstOrDefaultAsync<Collection>(query, new { Id = id });

    }
}
