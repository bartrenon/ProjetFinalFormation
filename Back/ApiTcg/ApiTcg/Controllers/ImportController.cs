using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using BLL.Interfaces;

namespace ApiTcg.Controllers;

[ApiController]
[Route("apiTcg/[controller]")]
public class ImportController : ControllerBase
{
    private readonly IImportService _importService;

    public ImportController(IImportService importService)
    {
        _importService = importService;
    }

    [Authorize]
    [HttpPost("sets")]
    public async Task<IActionResult> ImportSets([FromQuery] string lang = "fr")
    {
        int importedCount = await _importService.ImportSetsAsync(lang);

        return Ok(new { importedCount });
    }

    [Authorize]
    [HttpPost("cards")]
    public async Task<IActionResult> ImportCards([FromQuery] string lang = "fr")
    {
        int importedCount = await _importService.ImportCardsAsync(lang);

        return Ok(new { importedCount });
    }

    [HttpPost("price")]
    public async Task<IActionResult> SyncPrice(string cardId, [FromQuery] string lang = "fr")
    {
        int val = await _importService.ImportPricesForCardAsync(cardId, lang);

        if (val == 0) 
        {
            return NotFound($"Pas de pricing disponible pour {cardId}");
        }

        return Ok(val);
    }
}
