using BLL.Dtos.Set;

namespace BLL.Interfaces;

public interface ISetService
{
    Task<SetWithPaginationDto> GetFilteredSetsAsync(int userId, int pageNumber, int pageSize, string? name);
    Task<SetDetailDto?> GetByIdWithCardsAsync(string id, int userId);
}
