using BLL.Dtos.Card;
using BLL.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetFilteredCards(int pageNumber, int pageSize, string? name)
    {
        IEnumerable<CardSummaryDto> cards = await _cardService.GetFilteredCardsAsync(pageNumber, pageSize, name);

        return Ok(cards);
    }

    [Authorize]
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

    [Authorize]
    [HttpGet("Set/{id}")]
    public async Task<IActionResult> GetBySetIdAsync(string id)
    {
        IEnumerable<CardSummaryDto> cards = await _cardService.GetBySetIdAsync(id);

        if(cards is null) 
        {
            cards = new List<CardSummaryDto>();
        }

        return Ok(cards);
    }
}
