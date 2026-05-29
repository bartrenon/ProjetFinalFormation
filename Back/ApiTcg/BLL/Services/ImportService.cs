using BLL.Interfaces;
using DAL.Interfaces;
using Domain.Entities;
using Infrastructure.External;

namespace BLL.Services;

public class ImportService : IImportService
{
    private readonly TcgDexClient _tcgDexClient;
    private readonly ISetRepository _setRepository;

    public ImportService(TcgDexClient tcgDexClient, ISetRepository setRepository)
    {
        _tcgDexClient = tcgDexClient;
        _setRepository = setRepository;
    }

    public async Task<int> ImportSetsAsync(string lang = "fr")
    {
        var setsFromApi = await _tcgDexClient.GetAllSetsAsync(lang);
        int importedCount = 0;

        foreach (var setFromApi in setsFromApi)
        {
            Set set = new()
            {
                Id = setFromApi.Id,
                Name = setFromApi.Name,
                Logo = setFromApi.Logo,
                Symbol = setFromApi.Symbol,
                CardCountTotal = setFromApi.CardCount.Total,
                CardCountOfficial = setFromApi.CardCount.Official,
                CardCountReverse = setFromApi.CardCount.Reverse,
                CardCountHolo = setFromApi.CardCount.Holo,
                CardCountFirstEd = setFromApi.CardCount.FirstEd
            };

            await _setRepository.UpsertAsync(set);
            importedCount++;
        }

        return importedCount;
    }
}
