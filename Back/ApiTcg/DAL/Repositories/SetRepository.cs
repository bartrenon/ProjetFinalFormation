using DAL.Interfaces;
using Dapper;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DAL.Repositories;

public class SetRepository : ISetRepository
{
    private readonly string _connectionString;

    public SetRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task<IEnumerable<Set>> GetAllAsync()
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        const string query = "SELECT * FROM [Set] ORDER BY Name";

        return await connection.QueryAsync<Set>(query);
    }

    public async Task<Set?> GetByIdAsync(string id)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        const string query = "SELECT * FROM [Set] WHERE Id = @Id";

        return await connection.QueryFirstOrDefaultAsync<Set>(query, new { Id = id });
    }

    public async Task UpsertAsync(Set set)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        const string query = @" MERGE INTO [Set] AS target
                                USING (VALUES (
                                    @Id,
                                    @Name,
                                    @Logo,
                                    @Symbol,
                                    @CardCountTotal,
                                    @CardCountOfficial
                                ))
                                AS source (
                                    Id,
                                    Name,
                                    Logo,
                                    Symbol,
                                    CardCountTotal,
                                    CardCountOfficial
                                )
                                ON target.Id = source.Id
                                WHEN MATCHED THEN
                                    UPDATE SET
                                        Name = source.Name,
                                        Logo = source.Logo,
                                        Symbol = source.Symbol,
                                        CardCountTotal = source.CardCountTotal,
                                        CardCountOfficial = source.CardCountOfficial
                                WHEN NOT MATCHED THEN
                                    INSERT (
                                        Id,
                                        Name,
                                        Logo,
                                        Symbol,
                                        CardCountTotal,
                                        CardCountOfficial
                                    )
                                    VALUES (
                                        source.Id,
                                        source.Name,
                                        source.Logo,
                                        source.Symbol,
                                        source.CardCountTotal,
                                        source.CardCountOfficial
                                    );";

        await connection.ExecuteAsync(query, set);
    }
}
