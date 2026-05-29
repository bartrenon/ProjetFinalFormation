namespace BLL.Interfaces;

public interface IImportService
{
    Task<int> ImportSetsAsync(string lang = "fr");
}
