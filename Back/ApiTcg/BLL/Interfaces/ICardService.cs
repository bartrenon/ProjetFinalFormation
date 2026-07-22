using BLL.Dtos.Card;

namespace BLL.Interfaces;

public interface ICardService
{
    Task<CardWithPaginationDto> GetFilteredCardsAsync(int pageNumber, int pageSize, string? name, int userId);

    Task<CardDto?> GetByIdAsync(string id, int userId);

    Task<IEnumerable<CardSummaryDto>> GetBySetIdAsync(string setId, int userId);
}
