using Domain.Entities;

namespace DAL.Interfaces;

public interface ICardListingRepository
{
    Task<CardListing?> GetByIdAsync(int listingId);
    Task<IEnumerable<CardListing>> GetActiveAsync();
    Task<IEnumerable<CardListing>> GetBySellerAsync(int sellerId);
    Task<int> CreateAsync(CardListing listing);
    Task<bool> UpdateAsync(CardListing listing);
    Task<bool> DeleteAsync(int listingId);
    Task<bool> MarkAsSoldAsync(int listingId, int buyerId);
    Task<IEnumerable<CardListing>> GetByBuyerAsync(int buyerId);
}
