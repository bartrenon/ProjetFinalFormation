namespace BLL.Dtos.Card;

public class CardWithPaginationDto
{
    public IEnumerable<CardSummaryDto> cards { get; set; } = new List<CardSummaryDto>();

    public int totalCards { get; set; }
}
