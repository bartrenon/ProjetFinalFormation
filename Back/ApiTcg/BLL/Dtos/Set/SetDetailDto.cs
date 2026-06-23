using BLL.Dtos.Card;

namespace BLL.Dtos.Set;
public class SetDetailDto
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string? Logo { get; set; }
    public string? Symbol { get; set; }
    public int CardCountTotal { get; set; }
    public int CardCountOfficial { get; set; }
    public IEnumerable<CardSummaryDTO> Cards { get; set; } = [];
}
