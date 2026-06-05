namespace Domain.Entities;

public class UserCard
{
    public int Id { get; set; }

    public int UserId {  get; set; }
    public string CardId { get; set; } = "";

    public DateTime CreatedAt {  get; set; }

}
