using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

using Dapper;
using static Dapper.SqlMapper;

using DAL.Interfaces;
using Domain.Entities;


namespace DAL.Repositories;

public class SetRepository : ISetRepository
{
    private readonly string _connectionString;

    public SetRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task<(IEnumerable<Set> ,int)> GetFilteredSetsAsync(int offset, int pageSize, string? name)
    {

        using SqlConnection connection = new SqlConnection(_connectionString);

        object parameters = new { Offset = offset, PageSize = pageSize };

        string whereClause = "";

        if(name is not null) 
        {
            whereClause += "WHERE Name Like @Name ";
            name = $"%{name}%";
            parameters = new { Offset = offset, PageSize = pageSize, Name = name };
        }

        string query = $@" SELECT * FROM [Set] {whereClause}
                           ORDER BY Name 
                           OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
                           SELECT COUNT(*) FROM [Set] {whereClause};";

        GridReader datas = await connection.QueryMultipleAsync(query, parameters);

        IEnumerable<Set> sets = await datas.ReadAsync<Set>();
        int totalCount = await datas.ReadSingleAsync<int>();

        return(sets, totalCount);
    }

    public async Task<Set?> GetByIdWithCardsAsync(string id)
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
