using DAL.Interfaces;
using Dapper;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DAL.Repositories;

public class CardPriceRepository : ICardPriceRepository
{
    private readonly string _connectionString;

    public CardPriceRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task<CardPrice?> GetByCardIdAsync(string cardId)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        const string sql = "SELECT * FROM CardPrice WHERE CardId = @cardId";

        return await connection.QuerySingleOrDefaultAsync<CardPrice>(sql, new { cardId });
    }

    public async Task<decimal> GetTotalCollectionValueAsync(int userId)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        const string query = @"SELECT ISNULL(SUM(c.NbDuplicateCard * cp.[Avg]), 0)
                             FROM Collection c
                             INNER JOIN CardPrice cp ON cp.CardId = c.CardId
                             WHERE c.UserId = @UserId;";

        return await connection.ExecuteScalarAsync<decimal>(query, new { UserId = userId });
    }

    public async Task<decimal> GetTotalCollectionValueBySetAsync(int userId, string setId)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        const string query = @"SELECT ISNULL(SUM(col.NbDuplicateCard * cp.[Avg]), 0)
                               FROM Collection col
                               INNER JOIN Card ca      ON ca.Id = col.CardId
                               INNER JOIN CardPrice cp ON cp.CardId = col.CardId
                               WHERE col.UserId = @UserId
                               AND ca.SetId = @SetId;";
        
        return await connection.ExecuteScalarAsync<decimal>(query, new { UserId = userId, SetId = setId });
    }

    public async Task UpsertAsync(CardPrice price)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        const string checkSql = "SELECT COUNT(1) FROM CardPrice WHERE CardId = @CardId";
        int exists = await connection.ExecuteScalarAsync<int>(checkSql, new { price.CardId });

        if (exists > 0)
        {
            const string updateSql = """
                UPDATE CardPrice
                SET Avg = @Avg, Avg30 = @Avg30, UpdatedAt = @UpdatedAt
                WHERE CardId = @CardId
                """;
            await connection.ExecuteAsync(updateSql, price);
        }
        else
        {
            const string insertSql = """
                INSERT INTO CardPrice (CardId, Avg, Avg30, UpdatedAt)
                VALUES (@CardId, @Avg, @Avg30, @UpdatedAt)
                """;
            await connection.ExecuteAsync(insertSql, price);
        }
    }
}
