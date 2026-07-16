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

    public async Task<IEnumerable<Card>> GetFilteredCardsAsync(int offset, int pageSize, string? name)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        object parameters = new { Offset = offset, PageSize = pageSize };

        string query = "SELECT * FROM [Card] ";

        if (name is not null)
        {
            query += "WHERE Name Like @Name ";
            name = $"%{name}%";
            parameters = new { Offset = offset, PageSize = pageSize, Name = name };
        }

        query += "ORDER BY SetId, LocalId OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

        return await connection.QueryAsync<Card>(query, parameters);
    }

    public async Task<Card?> GetByIdAsync(string id)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        const string query = @"SELECT 
                               c.Id, c.Name, c.SetId, c.LocalId, c.Image,
                               s.Id, s.Name, s.Logo, s.Symbol, s.CardCountTotal, s.CardCountOfficial
                               FROM [Card] c
                               INNER JOIN [Set] s ON c.SetId = s.Id
                               WHERE c.Id = @Id";

        IEnumerable<Card> result = await connection.QueryAsync<Card, Set, Card> 
        ( query, (card, set) => {
            card.Set = set;
            return card;
        },
        new { Id = id }, splitOn: "Id" );

        return result.FirstOrDefault();
    }

    public async Task<IEnumerable<Card>> GetBySetIdAsync(string setId)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        const string query = "SELECT * FROM [Card] WHERE SetId = @Id ORDER BY CAST(LocalId AS INT)";

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
