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
