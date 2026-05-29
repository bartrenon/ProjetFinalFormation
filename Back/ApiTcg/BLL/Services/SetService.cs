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

    public async Task<IEnumerable<Set>> GetAllAsync()
    {
        return await _setRepository.GetAllAsync();
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
