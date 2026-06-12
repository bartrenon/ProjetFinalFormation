using BLL.Interfaces;
using DAL.Interfaces;
using Domain.Entities;

namespace BLL.Services;

public class SetService : ISetService
{
    private readonly ISetRepository _setRepository;

    public SetService(ISetRepository setRepository)
    {
        _setRepository = setRepository;
    }

    public async Task<IEnumerable<Set>> GetFilteredSetsAsync(int pageNumber, int pageSize, string? name)
    {
        int offset = (pageNumber - 1) * pageSize;

        return await _setRepository.GetFilteredSetsAsync(offset, pageSize, name);
    }

    public async Task<Set?> GetByIdAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return null;
        }

        return await _setRepository.GetByIdAsync(id);
    }
}
