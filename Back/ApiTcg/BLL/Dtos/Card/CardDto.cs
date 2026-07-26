using BLL.Dtos.Collection;
using BLL.Dtos.Set;
using Domain.Entities;

namespace BLL.Dtos.Card;

public class CardDto
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string SetId { get; set; } = "";
    public string? LocalId { get; set; }
    public string? Image { get; set; }

    public SetSummaryDto Set { get; set; } = new SetSummaryDto();
    public CollectionSummaryDto? Collection { get; set; } = new CollectionSummaryDto();
    public CardPrice? Price { get; set; } = new CardPrice();
}
