namespace Domain.Entities;

public class Collection
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string CardId { get; set; } = "";
    public int NbDuplicateCard { get; set; }
    public DateTime CreatedAt { get; set; }

    public User? User { get; set; }
    public Card? Card { get; set; }
}
