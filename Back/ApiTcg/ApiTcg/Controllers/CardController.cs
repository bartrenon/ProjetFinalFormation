using BLL.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ApiTcg.Controllers;

[ApiController]
[Route("apiTcg/[controller]")]
public class CardController :ControllerBase
{
    private readonly ICardService _cardService;

    public CardController(ICardService cardService) 
    {
        _cardService = cardService;
    }

    [HttpGet]
    public async Task<IActionResult> GetFilteredCards(int pageNumber, int pageSize, string? name)
    {
        IEnumerable<Card> cards = await _cardService.GetFilteredCardsAsync(pageNumber, pageSize, name);

        return Ok(cards);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        Card? card = await _cardService.GetByIdAsync(id);

        if (card is null)
        {
            return NotFound();
        }

        return Ok(card);
    }

    [HttpGet("Set/{id}")]
    public async Task<IActionResult> GetBySetIdAsync(string id)
    {
        IEnumerable<Card> cards = await _cardService.GetBySetIdAsync(id);

        if(cards is null) 
        {
            cards = new List<Card>();
        }

        return Ok(cards);
    }
}
