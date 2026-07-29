using Domain.Enum;

namespace BLL.Dtos.CardListing;

public class UpdateCardListingDto
{
    public decimal?  Price { get; set; }
    public string? Description { get; set; } = "";

    public ListingStatus? Status { get; set; } 
}
