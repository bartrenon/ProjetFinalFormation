namespace BLL.Interfaces;

public interface IImportService
{
    Task<int> ImportSetsAsync(string lang = "fr");

    Task<int> ImportCardsAsync(string lang = "fr");

    Task<int> ImportPricesForCardAsync(string cardId, string lang = "fr");

}
