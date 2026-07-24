namespace Domain.Entities;
public class CardPrice
{
    public int Id { get; set; }
    public string CardId { get; set; } = "";
    public decimal? Avg { get; set; }
    public decimal? Avg30 { get; set; }
    public DateTime UpdatedAt { get; set; }
}
