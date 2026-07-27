using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using BLL.Dtos.User;
using BLL.Interfaces;

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

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserCreateDto user) 
    {

        int val = await _userService.RegisterAsync(user);

        if (val == 1){
            return Ok(val);
        }
      
        return BadRequest("User creation failed.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginDto userLogin)
    {
        UserJwtDto? result = await _userService.LoginAsync(userLogin);

        if (result is null)
        {
            return Unauthorized("Email ou mot de passe incorrect.");
        }

        return Ok(result);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        UserSummaryDto? result = await _userService.GetByIdAsync(id);

        if (result is null)
        {
            return NotFound("Infos introuvable");
        }

        return Ok(result);
    }


    [Authorize]
    [HttpPatch("DeleteUser/{id}")]
    public async Task<IActionResult> SoftDeleteUser(int id)
    {
        int result = await _userService.SoftDeleteUserAsync(id);

        if (result == 1){
            return NoContent();
        }

        return NotFound("User not found.");
    }

    [Authorize]
    [HttpDelete("DeleteUser/{id}")]
    public async Task<IActionResult> HardDeleteUser(int id)
    {
        int result = await _userService.HardDeleteUserAsync(id);

        if (result == 1){
            return NoContent();
        }
       
        return NotFound("User not found.");
    }

    [Authorize]
    [HttpDelete("DeletedUsers")]
    public async Task<IActionResult> HardDeleteUser(DateTime? deltedDate)
    {
        int result = await _userService.HardDeleteUserAsync(deltedDate);

        if (result == 1){
            return NoContent();
        }

        return BadRequest();
    }

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

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(UserJwtRequestDto request)
    {
        UserJwtDto? result = await _userService.RefreshTokenAsync(request.RefreshToken);

        if (result is null)
        {
            return Unauthorized("Refresh token invalide ou expiré.");
        }

        return Ok(result);
    }

    [HttpPost("revoke-token")]
    public async Task<IActionResult> RevokeToken(UserJwtRequestDto request)
    {
        bool result = await _userService.RevokeRefreshTokenAsync(request.RefreshToken);

        if (!result)
        {
            return NotFound("Refresh token introuvable ou déjà révoqué.");
        }

        return NoContent();
    }

}
