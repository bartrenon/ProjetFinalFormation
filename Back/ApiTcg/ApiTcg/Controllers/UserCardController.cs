using ApiTcg.Dtos.User;
using ApiTcg.Mappers;
using BLL.Interfaces;
using BLL.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ApiTcg.Controllers;

[ApiController]
[Route("apiTcg/[controller]")]
public class UserCardController : ControllerBase
{
    private readonly IUserCardService _userCardService;

    public UserCardController(IUserCardService userCardService) 
    {
        this._userCardService = userCardService;
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> DeleteUserCard(string id)
    {
        int result = await _userCardService.DeleteUserCardAsync(id);

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
    public async Task<IActionResult> AddUserCard(UserCreateDto u)
    {
        UserCard userCard = UserCardMapper.ToUserCard(u);

        int val = await _userCardService.AddUserCardAsync(userCard);

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


