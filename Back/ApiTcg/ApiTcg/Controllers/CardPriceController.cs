using System.Security.Claims;
using BLL.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ApiTcg.Controllers;


[ApiController]
[Route("apiTcg/[controller]")]
public class CardPriceController : ControllerBase
{
    private readonly ICardPriceService _cardPriceService;

    public CardPriceController(ICardPriceService cardPriceService)
    {
        _cardPriceService = cardPriceService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPriceByIdCard(string id)
    {

        CardPrice? price = await _cardPriceService.GetByCardIdAsync(id);

        if (price is null || (price.Avg == 0 && price.Avg30 == 0))
            return NotFound("Aucun prix trouvé pour cette carte");

        return Ok(price);

    }

    [HttpGet("value")]
    public async Task<ActionResult<decimal>> GetTotalValue()
    {
        int.TryParse(User.FindFirstValue("id"), out int userId);

        decimal result = await _cardPriceService.GetTotalCollectionValueAsync(userId);

        return Ok(result);
    }

    [HttpGet("value/set/{setId}")]
    public async Task<ActionResult<decimal>> GetTotalValueBySet(string setId)
    {
        int.TryParse(User.FindFirstValue("id"), out int userId);

        decimal result = await _cardPriceService.GetTotalValueBySetAsync(userId, setId);

        return Ok(result);
    }
}

