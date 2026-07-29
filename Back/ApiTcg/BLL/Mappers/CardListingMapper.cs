using BLL.Dtos.CardListing;
using Domain.Entities;
using Domain.Enum;

namespace BLL.Mappers;

public class CardListingMapper
{
    public static CardListingResponseDto ToCardListingResponseDto(CardListing listing)
    {
        return new CardListingResponseDto
        {
             ListingId  = listing.ListingId,
             CardId = listing.CardId,
             Price = listing.Price,
             SellerId = listing.SellerId,
             BuyerId = listing.BuyerId,
             Status = listing.Status.ToString(),
             CreatedDate = listing.CreatedDate,
             ModifiedDate = listing.ModifiedDate,
             Description = listing.Description
        };
    }

    public static CardListing ToCardListing(CreateCardListingDto newListing)
    {
        return new CardListing
        {
            CardId = newListing.CardId,
            Price = newListing.Price,
            SellerId = newListing.SellerId,
            Description = newListing.Description,
            Status = ListingStatus.Active
        };
    }
}
