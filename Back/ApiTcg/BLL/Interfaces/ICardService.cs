using BLL.Dtos.Card;
using Domain.Entities;

namespace BLL.Interfaces;

public interface ICardService
{
    Task<IEnumerable<CardSummaryDto>> GetFilteredCardsAsync(int pageNumber, int pageSize, string? name);

    Task<Card?> GetByIdAsync(string id);

    Task<IEnumerable<CardSummaryDto>> GetBySetIdAsync(string setId);
}
