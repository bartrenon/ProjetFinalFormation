namespace Domain.Entities;

public class Card
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string SetId { get; set; } = "";       
    public string? LocalId { get; set; }          
    public string? Image { get; set; }


    public Set Set { get; set; } = new Set();
    public ICollection<Collection> Collections { get; set; } = [];
}
