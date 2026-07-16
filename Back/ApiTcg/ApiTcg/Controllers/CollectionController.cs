using BLL.Dtos.Collection;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize]
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

    [Authorize]
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

    [Authorize]
    [HttpGet("user/{userId}/card/{cardId}")]
    public async Task<IActionResult> GetById(int userId, string cardId) 
    {
        CollectionSummaryDto? collection = await _collectionService.GetByIdAsync(userId, cardId);

        if(collection is null) 
        {
            return NotFound();
        }

        return Ok(collection);
    }

    [Authorize]
    [HttpPatch("DeleteUser/{id}")]
    public async Task<IActionResult> UpdateCollection(int id, bool isAdding)
    {
        int result = await _collectionService.UpdateCollectionAsync(id,isAdding);

        if (result == 1)
        {
            return NoContent();
        }
        else
        {
            return NotFound("Colletion not found");
        }
    }
}
