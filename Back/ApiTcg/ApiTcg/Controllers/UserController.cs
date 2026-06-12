using BLL.Dtos.User;

using BLL.Interfaces;

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

    //POST

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserCreateDto user) 
    {

        int val = await _userService.RegisterAsync(user);

        if (val == 1)
        {
            return Ok(val);
        }
        else
        {
            return BadRequest("User creation failed.");
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginDto userLogin)
    {
        string? token = await _userService.LoginAsync(userLogin);

        if (token is null)
        {
            return Unauthorized("Email ou mot de passe incorrect.");
        }

        return Ok(new { token });
    }

    //PATCH
    [HttpPatch("DeleteUser/{id}")]
    public async Task<IActionResult> SoftDeleteUser(int id)
    {
        int result = await _userService.SoftDeleteUserAsync(id);

        if (result == 1)
        {
            return NoContent();
        }
        else
        {
            return NotFound("User not found.");
        }
    }


    //DELETE
    [HttpDelete("DeleteUser/{id}")]
    public async Task<IActionResult> HardDeleteUser(int id)
    {
        int result = await _userService.HardDeleteUserAsync(id);

        if (result == 1)
        {
            return NoContent();
        }
        else
        {
            return NotFound("User not found.");
        }
    }

    [HttpDelete("DeletedUsers")]
    public async Task<IActionResult> HardDeleteUser(DateTime? deltedDate)
    {
        int result = await _userService.HardDeleteUserAsync(deltedDate);

        if (result == 1)
        {
            return NoContent();
        }
        else
        {
            return BadRequest();
        }
    }

    //Get
    [HttpGet("check-email")]
    public async Task<IActionResult> CheckEmail([FromQuery] string email)
    {
        bool taken = await _userService.IsEmailTakenAsync(email);
        return Ok(new {available = !taken});
    }

    [HttpGet("check-username")]
    public async Task<IActionResult> CheckUsername([FromQuery] string username)
    {
        bool taken = await _userService.IsUsernameTakenAsync(username);
        return Ok(new { available = !taken });
    }

}
