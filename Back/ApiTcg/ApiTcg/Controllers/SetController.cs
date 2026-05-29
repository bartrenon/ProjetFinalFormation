using BLL.Interfaces;
using BLL.Services;
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
    public async Task<IActionResult> GetAll()
    {
        var sets = await _setService.GetAllAsync();

        return Ok(sets);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var set = await _setService.GetByIdAsync(id);

        if (set is null)
        {
            return NotFound();
        }

        return Ok(set);
    }
}
