using System.Security.Claims;
using BLL.Dtos.CardListing;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiTcg.Controllers;

[ApiController]
[Route("apiTcg/[controller]")]
public class CardListingController : ControllerBase
{
    private readonly ICardListingService _service;

    public CardListingController(ICardListingService service)
    {
        _service = service;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var listing = await _service.GetByIdAsync(id);
        return listing is null ? NotFound() : Ok(listing);
    }

    [HttpGet]
    public async Task<IActionResult> GetActive()
    {
        var listings = await _service.GetActiveAsync();
        return Ok(listings);
    }

    [HttpGet("seller/{sellerId:int}")]
    public async Task<IActionResult> GetBySeller(int sellerId)
    {
        var listings = await _service.GetBySellerAsync(sellerId);
        return Ok(listings);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCardListingDto dto)
    {
        int.TryParse(User.FindFirstValue("id"), out int userId);

        try
        {
            dto.SellerId = userId;
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.ListingId }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCardListingDto dto)
    {
        int.TryParse(User.FindFirstValue("id"), out int userId);

        var listing = await _service.GetByIdAsync(id);

        if (listing is null)
            return NotFound();

        if (listing.SellerId != userId)
            return Forbid();

        var success = await _service.UpdateAsync(id, dto);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        int.TryParse(User.FindFirstValue("id"), out int userId);

        var listing = await _service.GetByIdAsync(id);

        if (listing is null)
            return NotFound();

        if (listing.SellerId != userId)
            return Forbid();

        var success = await _service.DeleteAsync(id);
        return success ? NoContent() : NotFound();
    }

    [HttpPost("{id:int}/buy")]
    public async Task<IActionResult> Buy(int id)
    {
        int.TryParse(User.FindFirstValue("id"), out int buyerId);

        try
        {
            var success = await _service.BuyAsync(id, buyerId);
            return success ? Ok() : BadRequest("Annonce non disponible.");
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

}
