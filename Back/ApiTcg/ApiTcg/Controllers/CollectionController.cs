using ApiTcg.Dtos.Collection;
using ApiTcg.Mappers;
using BLL.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ApiTcg.Controllers;

[ApiController]
[Route("apiTcg/[controller]")]
public class CollectionController : ControllerBase
{
    private readonly ICollectionService _collectionService;

    public CollectionController(ICollectionService collectionService) 
    {
        this._collectionService = collectionService;
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> DeleteCollection(string id)
    {
        int result = await _collectionService.DeleteCollectionAsync(id);

        if (result == 1)
        {
            return NoContent();
        }
        else
        {
            return NotFound("Data not found.");
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> AddCollection(CollectionCreateDto u)
    {
        Collection collection = CollectionMapper.ToCollection(u);

        int val = await _collectionService.AddCollectionAsync(collection);

        if (val == 1)
        {
            return Ok(val);
        }
        else
        {
            return BadRequest("add card at collection failed.");
        }
    }
}


