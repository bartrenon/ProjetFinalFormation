using DAL.Interfaces;
using Dapper;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DAL.Repositories;

public class CardRepository : ICardRepository
{
    private readonly string _connectionString;

    public CardRepository(IConfiguration configuration) 
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task<IEnumerable<Card>> GetAllAsync()
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        const string query = "SELECT * FROM [Card] ORDER BY SetId, LocalId";

        return await connection.QueryAsync<Card>(query);
    }

    public async Task<Card?> GetByIdAsync(string id)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        const string query = "SELECT * FROM [Card] WHERE LocalId = @Id";

        return await connection.QueryFirstOrDefaultAsync<Card>(query, new { Id = id });
    }

    public async Task<IEnumerable<Card>> GetBySetIdAsync(string setId)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        const string query = "SELECT * FROM [Card] WHERE SetId = @Id";

        return await connection.QueryAsync<Card>(query, new { Id = setId });
    }

    public async Task UpsertAsync(Card card)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        const string query = @" MERGE INTO [Card] AS target
                                USING (VALUES (
                                    @Id,
                                    @Name,
                                    @SetId ,
                                    @LocalId ,
                                    @Image
                                ))
                                AS source (
                                    Id,
                                    Name,
                                    SetId,
                                    LocalId,
                                    Image
                                )
                                ON target.Id = source.Id
                                WHEN MATCHED THEN
                                    UPDATE SET
                                        Name = source.Name,
                                        SetId = source.SetId,
                                        LocalId = source.LocalId,
                                        Image = source.Image
                                WHEN NOT MATCHED THEN
                                    INSERT (
                                        Id,
                                        Name,
                                        SetId,
                                        LocalId,
                                        Image
                                    )
                                    VALUES (
                                        source.Id,
                                        source.Name,
                                        source.SetId,
                                        source.LocalId,
                                        source.Image
                                    );";

        await connection.ExecuteAsync(query, card);
    }
}
