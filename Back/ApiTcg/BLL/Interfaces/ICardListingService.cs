using BLL.Dtos.CardListing;

namespace BLL.Interfaces;

public interface ICardListingService
{
    Task<CardListingResponseDto?> GetByIdAsync(int listingId);
    Task<IEnumerable<CardListingResponseDto>> GetActiveAsync();
    Task<IEnumerable<CardListingResponseDto>> GetBySellerAsync(int sellerId);
    Task<CardListingResponseDto> CreateAsync(CreateCardListingDto dto);
    Task<bool> UpdateAsync(int listingId, UpdateCardListingDto dto);
    Task<bool> DeleteAsync(int listingId);
    Task<bool> BuyAsync(int listingId, int buyerId);
    Task<IEnumerable<CardListingResponseDto>> GetByBuyerAsync(int buyerId);
}
