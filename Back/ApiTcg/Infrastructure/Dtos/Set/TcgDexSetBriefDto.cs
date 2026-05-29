namespace Infrastructure.Dtos.Set;

public class TcgDexSetBriefDto
{
    public required string Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Logo { get; set; }
    public string? Symbol { get; set; }

    public CardCountDto CardCount { get; set; } = new();
}
