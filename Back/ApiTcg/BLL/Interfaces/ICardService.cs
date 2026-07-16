using BLL.Dtos.Card;

namespace BLL.Interfaces;

public interface ICardService
{
    Task<IEnumerable<CardSummaryDto>> GetFilteredCardsAsync(int pageNumber, int pageSize, string? name);

    Task<CardDto?> GetByIdAsync(string id, int userId);

    Task<IEnumerable<CardSummaryDto>> GetBySetIdAsync(string setId);
}
