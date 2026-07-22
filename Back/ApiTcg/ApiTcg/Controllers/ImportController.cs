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
}
