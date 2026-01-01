using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sports_reservation_system.Business.Common;
using sports_reservation_system.Business.DTOs.UserDtos;
using sports_reservation_system.Business.Services;

namespace sports_reservation_system.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize] // JWT Authentication gerekli
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    // GET: api/users
    [HttpGet]
    [Authorize(Roles = "Admin")] // Sadece Admin tüm kullanıcıları görebilir
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAllUsersAsync();
        var response = ApiResponse<IEnumerable<UserDto>>.SuccessResponse(users, "Kullanıcılar başarıyla getirildi.");
        return Ok(response);
    }

    // GET: api/users/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            var errorResponse = ApiResponse<UserDto>.ErrorResponse($"ID'si {id} olan kullanıcı bulunamadı.");
            return NotFound(errorResponse);
        }

        var response = ApiResponse<UserDto>.SuccessResponse(user, "Kullanıcı başarıyla getirildi.");
        return Ok(response);
    }

    // PUT: api/users/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto updateUserDto)
    {
        await _userService.UpdateUserAsync(id, updateUserDto);
        var response = ApiResponse<object>.SuccessResponse(null, "Kullanıcı başarıyla güncellendi.");
        return Ok(response);
    }

    // DELETE: api/users/{id}
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")] // Sadece Admin silebilir
    public async Task<IActionResult> Delete(int id)
    {
        await _userService.DeleteUserAsync(id);
        return NoContent(); // 204 No Content
    }
}

