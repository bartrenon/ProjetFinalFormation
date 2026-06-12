using BLL.Dtos.Collection;
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
    public async Task<IActionResult> DeleteCollection(int id)
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

    [HttpPost("Add")]
    public async Task<IActionResult> AddCollection(  CollectionAddDto collection)
    {

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

    [HttpGet("user/{userId}/card/{cardId}")]
    public async Task<IActionResult> GetById(int userId, string cardId) 
    {
        Collection? collection = await _collectionService.GetByIdAsync(userId, cardId);

        if(collection is null) 
        {
            return NotFound();
        }

        return Ok(collection);
    }
}


