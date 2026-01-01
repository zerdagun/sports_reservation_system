using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sports_reservation_system.Business.Common;
using sports_reservation_system.Business.DTOs.UserDtos;
using sports_reservation_system.Business.Services;

namespace sports_reservation_system.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    // POST: api/auth/login
    [HttpPost("login")]
    [AllowAnonymous] // Login için authentication gerekmez
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var loginResponse = await _userService.LoginAsync(loginDto);
        var response = ApiResponse<LoginResponseDto>.SuccessResponse(loginResponse, "Giriş başarılı.");
        return Ok(response);
    }

    // POST: api/auth/register
    [HttpPost("register")]
    [AllowAnonymous] // Kayıt için authentication gerekmez
    public async Task<IActionResult> Register([FromBody] CreateUserDto createUserDto)
    {
        var user = await _userService.AddUserAsync(createUserDto);
        var response = ApiResponse<UserDto>.SuccessResponse(user, "Kullanıcı başarıyla kaydedildi.");
        return StatusCode(201, response);
    }
}

