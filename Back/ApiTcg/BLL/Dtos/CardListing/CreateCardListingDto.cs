namespace BLL.Dtos.CardListing;

public class CreateCardListingDto
{
    public string CardId { get; set; } = "";
    public decimal Price { get; set; }
    public int SellerId { get; set; }
    public string? Description { get; set; } = "";
}
