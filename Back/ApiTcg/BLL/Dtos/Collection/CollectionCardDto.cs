namespace BLL.Dtos.Collection;

public class CollectionCardDto
{
    public int Id { get; set; }
    public string CardId { get; set; } = "";
    public string? CardName { get; set; }
    public string? CardImage { get; set; }
    public int NbDuplicateCard { get; set; }
}
