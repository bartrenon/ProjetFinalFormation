using ApiTcg.DTO.User;
using ApiTcg.Mappers;

using BLL.Interfaces;
using Domain.Entities;

using Microsoft.AspNetCore.Mvc;

namespace ApiTcg.Controllers;

[ApiController]
[Route("apiTcg/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        this._userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserCreate u) 
    {
        User user = UserMapper.ToUser(u);

        int val = await _userService.CreateAsync(user);

        if (val == 1)
        {
            return Ok(val);
        }
        else
        {
            return BadRequest("User creation failed.");
        }
    }
}
