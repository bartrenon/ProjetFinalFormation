namespace BLL.Dtos.CardListing;

public class CardListingResponseDto
{
    public int ListingId { get; set; }
    public string CardId { get; set; } = "";
    public decimal Price { get; set; }
    public int SellerId { get; set; }
    public int? BuyerId { get; set; }
    public string? Status { get; set; } = "";
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string? Description { get; set; } = "";
    public string? CardName { get; set; }
    public string? CardImage { get; set; }
}
