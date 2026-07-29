using Domain.Enum;

namespace Domain.Entities;

public class CardListing
{
    public int ListingId { get; set; }
    public string CardId { get; set; } = "";
    public decimal Price { get; set; }
    public int SellerId { get; set; }
    public int? BuyerId { get; set; }
    public ListingStatus Status { get; set; } = ListingStatus.Active;
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string? Description { get; set; }
}
