using System.Security.Claims;
using BLL.Dtos.CardListing;
using BLL.Interfaces;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiTcg.Controllers;

[ApiController]
[Authorize]
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
    public async Task<IActionResult> GetActive([FromQuery] int page = 1, [FromQuery] int pageSize = 12, [FromQuery] string? q = null)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var listings = (await _service.GetActiveAsync()).AsEnumerable();

        if (!string.IsNullOrWhiteSpace(q))
        {
            string query = q.Trim();
            listings = listings.Where(listing => listing.CardId.Contains(query, StringComparison.OrdinalIgnoreCase));
        }

        int totalListings = listings.Count();
        var pageListings = listings
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        return Ok(new { listings = pageListings, totalListings });
    }

    [HttpGet("seller/{sellerId:int}")]
    public async Task<IActionResult> GetBySeller(int sellerId, [FromQuery] int page = 1, [FromQuery] int pageSize = 12)
    {
        int.TryParse(User.FindFirstValue("id"), out int userId);
        if (sellerId != userId)
            return Forbid();

        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var listings = (await _service.GetBySellerAsync(sellerId)).ToList();
        int totalListings = listings.Count;
        var pageListings = listings
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        return Ok(new { listings = pageListings, totalListings });
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

    [Authorize]
    [HttpGet("buyer")]
    public async Task<IActionResult> GetByBuyer()
    {
        int.TryParse(User.FindFirstValue("id"), out int buyerId);

        IEnumerable<CardListingResponseDto> listings = await _service.GetByBuyerAsync(buyerId);
        return Ok(listings);
    }

}
