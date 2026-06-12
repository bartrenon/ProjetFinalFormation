using BLL.Interfaces;
using Domain.Entities;
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

    [HttpGet]
    public async Task<IActionResult> GetFilteredSets(int pageNumber, int pageSize, string? name)
    {
        IEnumerable<Set> sets = await _setService.GetFilteredSetsAsync(pageNumber, pageSize, name);

        return Ok(sets);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        Set? set = await _setService.GetByIdWithCardsAsync(id);

        if (set is null)
        {
            return NotFound();
        }

        return Ok(set);
    }
}
