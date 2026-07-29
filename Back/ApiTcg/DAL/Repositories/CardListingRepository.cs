using DAL.Interfaces;
using Dapper;
using Domain.Entities;
using Domain.Enum;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DAL.Repositories;

public class CardListingRepository : ICardListingRepository
{
    private readonly string _connectionString;

    public CardListingRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }


    public async Task<int> CreateAsync(CardListing listing)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        const string sql = @"
            INSERT INTO CardListing (CardId, Price, SellerId, Status, Description)
            OUTPUT INSERTED.ListingId
            VALUES (@CardId, @Price, @SellerId, @Status, @Description)";

        return await connection.ExecuteScalarAsync<int>(sql, new
        {
            listing.CardId,
            listing.Price,
            listing.SellerId,
            Status = ListingStatus.Active.ToString(),
            listing.Description
        });
    }

    public async Task<bool> DeleteAsync(int listingId)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        const string sql = @"
            UPDATE CardListing
            SET Status = @Status, ModifiedDate = SYSDATETIME()
            WHERE ListingId = @ListingId";

        int affected = await connection.ExecuteAsync(sql, new
        {
            ListingId = listingId,
            Status = ListingStatus.Removed.ToString()
        });

        return affected > 0;
    }

    public async Task<IEnumerable<CardListing>> GetActiveAsync()
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        const string sql = @"
        SELECT ListingId, CardId, Price, SellerId, BuyerId, Status,
               CreatedDate, ModifiedDate, Description
        FROM CardListing
        WHERE Status = @Status
        ORDER BY CreatedDate DESC";

        return await connection.QueryAsync<CardListing>(sql, new { Status = ListingStatus.Active.ToString() });
    }

    public async Task<CardListing?> GetByIdAsync(int listingId)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        const string sql = @"
            SELECT ListingId, CardId, Price, SellerId, BuyerId, Status,
                   CreatedDate, ModifiedDate, Description
            FROM CardListing
            WHERE ListingId = @ListingId";

        return await connection.QuerySingleOrDefaultAsync<CardListing>(sql, new { ListingId = listingId });
    }

    public async Task<IEnumerable<CardListing>> GetBySellerAsync(int sellerId)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        const string sql = @"
            SELECT ListingId, CardId, Price, SellerId, BuyerId, Status,
                   CreatedDate, ModifiedDate, Description
            FROM CardListing
            WHERE SellerId = @SellerId
            ORDER BY CreatedDate DESC";

        return await connection.QueryAsync<CardListing>(sql, new { SellerId = sellerId });
    }

    public async Task<bool> MarkAsSoldAsync(int listingId, int buyerId)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        const string sql = @"
        UPDATE CardListing
        SET Status = @Status, BuyerId = @BuyerId, ModifiedDate = SYSDATETIME()
        WHERE ListingId = @ListingId AND Status = @ActiveStatus";

        var affected = await connection.ExecuteAsync(sql, new
        {
            ListingId = listingId,
            BuyerId = buyerId,
            Status = ListingStatus.Sold.ToString(),
            ActiveStatus = ListingStatus.Active.ToString()
        });

        return affected > 0;
    }

    public async Task<bool> UpdateAsync(CardListing listing)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        const string sql = @"
            UPDATE CardListing
            SET Price = @Price,
                Description = @Description,
                Status = @Status,
                ModifiedDate = SYSDATETIME()
            WHERE ListingId = @ListingId";

        int affected = await connection.ExecuteAsync(sql, new
        {
            listing.Price,
            listing.Description,
            Status = listing.Status.ToString(),
            listing.ListingId
        });

        return affected > 0;
    }
}
