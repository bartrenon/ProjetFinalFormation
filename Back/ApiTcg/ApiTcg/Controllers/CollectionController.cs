using System.Security.Claims;
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
    [HttpDelete("delete/{id}")]
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
    [HttpPost("{cardId}")]
    public async Task<IActionResult> AddCollection(string cardId)
    {
        int.TryParse(User.FindFirstValue("id"), out int userId);

        int val = await _collectionService.AddCollectionAsync(userId, cardId);

        return Ok(val);
    }

    [Authorize]
    [HttpGet("card/{cardId}")]
    public async Task<IActionResult> GetById(string cardId) 
    {
        int.TryParse(User.FindFirstValue("id"), out int userId);

        CollectionSummaryDto? collection = await _collectionService.GetByIdAsync(userId, cardId);

        if(collection is null) 
        {
            return NotFound();
        }

        return Ok(collection);
    }

    [Authorize]
    [HttpPatch("{id}/{isAdding}")]
    public async Task<IActionResult> UpdateCollection(int id, bool isAdding)
    {
        int result = await _collectionService.UpdateCollectionAsync(id,isAdding);

        if (result == 1)
        {
            return NoContent();
        }
        else
        {
            return NotFound("Card is not in your collection");
        }
    }
}
