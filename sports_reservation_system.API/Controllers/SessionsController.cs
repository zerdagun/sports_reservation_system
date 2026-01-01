using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sports_reservation_system.Business.Common;
using sports_reservation_system.Business.DTOs.SessionDtos;
using sports_reservation_system.Business.Services;

namespace sports_reservation_system.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize] // JWT Authentication gerekli
public class SessionsController : ControllerBase
{
    private readonly ISessionService _sessionService;

    public SessionsController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    // GET: api/sessions
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var sessions = await _sessionService.GetAllSessionsAsync();
        var response = ApiResponse<IEnumerable<SessionDto>>.SuccessResponse(sessions, "Seanslar başarıyla getirildi.");
        return Ok(response);
    }

    // GET: api/sessions/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var session = await _sessionService.GetSessionByIdAsync(id);
        if (session == null)
        {
            var errorResponse = ApiResponse<SessionDto>.ErrorResponse($"ID'si {id} olan seans bulunamadı.");
            return NotFound(errorResponse);
        }

        var response = ApiResponse<SessionDto>.SuccessResponse(session, "Seans başarıyla getirildi.");
        return Ok(response);
    }

    // POST: api/sessions
    [HttpPost]
    [Authorize(Roles = "Admin")] // Sadece Admin ekleyebilir
    public async Task<IActionResult> Add([FromBody] CreateSessionDto sessionDto)
    {
        var session = await _sessionService.AddSessionAsync(sessionDto);
        var response = ApiResponse<SessionDto>.SuccessResponse(session, "Seans başarıyla eklendi.");
        return StatusCode(201, response);
    }

    // PUT: api/sessions/{id}
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")] // Sadece Admin güncelleyebilir
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSessionDto sessionDto)
    {
        await _sessionService.UpdateSessionAsync(id, sessionDto);
        var response = ApiResponse<object>.SuccessResponse(null, "Seans başarıyla güncellendi.");
        return Ok(response);
    }

    // DELETE: api/sessions/{id}
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")] // Sadece Admin silebilir
    public async Task<IActionResult> Delete(int id)
    {
        await _sessionService.DeleteSessionAsync(id);
        return NoContent(); // 204 No Content
    }
}

