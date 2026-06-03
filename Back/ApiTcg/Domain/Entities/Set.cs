namespace Domain.Entities;

public class Set
{
    public required string Id { get; set; }
    public string Name { get; set; } = "";
    public string? Logo { get; set; }
    public string? Symbol { get; set; }
    public int CardCountTotal { get; set; }
    public int CardCountOfficial { get; set; }
}
