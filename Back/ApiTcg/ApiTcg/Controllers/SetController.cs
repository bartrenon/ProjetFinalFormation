using BLL.Dtos.Set;
using BLL.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiTcg.Controllers;

[ApiController]
[Route("apiTcg/[controller]")]
public class SetController : ControllerBase
{
    private readonly ISetService _setService;

    public SetController(ISetService setService)
    {
        _setService = setService;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetFilteredSets(int pageNumber, int pageSize, string? name)
    {
        IEnumerable<Set> sets = await _setService.GetFilteredSetsAsync(pageNumber, pageSize, name);

        return Ok(sets);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdWithCards(string id)
    {
        SetDetailDto? set = await _setService.GetByIdWithCardsAsync(id);

        if (set is null)
        {
            return NotFound();
        }

        return Ok(set);
    }
}
