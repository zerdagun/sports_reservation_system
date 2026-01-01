using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sports_reservation_system.Business.Common;
using sports_reservation_system.Business.DTOs.ReservationDtos;
using sports_reservation_system.Business.Services;

namespace sports_reservation_system.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize] // JWT Authentication gerekli
public class ReservationsController : ControllerBase
{
    private readonly IReservationService _reservationService;

    public ReservationsController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    // GET: api/reservations
    [HttpGet]
    [Authorize(Roles = "Admin")] // Sadece Admin tüm rezervasyonları görebilir
    public async Task<IActionResult> GetAll()
    {
        var reservations = await _reservationService.GetAllReservationsAsync();
        var response = ApiResponse<IEnumerable<ReservationDto>>.SuccessResponse(reservations, "Rezervasyonlar başarıyla getirildi.");
        return Ok(response);
    }

    // GET: api/reservations/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var reservation = await _reservationService.GetReservationByIdAsync(id);
        if (reservation == null)
        {
            var errorResponse = ApiResponse<ReservationDto>.ErrorResponse($"ID'si {id} olan rezervasyon bulunamadı.");
            return NotFound(errorResponse);
        }

        var response = ApiResponse<ReservationDto>.SuccessResponse(reservation, "Rezervasyon başarıyla getirildi.");
        return Ok(response);
    }

    // POST: api/reservations
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateReservationDto reservationDto)
    {
        var reservation = await _reservationService.AddReservationAsync(reservationDto);
        var response = ApiResponse<ReservationDto>.SuccessResponse(reservation, "Rezervasyon başarıyla oluşturuldu.");
        return StatusCode(201, response);
    }

    // PUT: api/reservations/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateReservationDto reservationDto)
    {
        await _reservationService.UpdateReservationAsync(id, reservationDto);
        var response = ApiResponse<object>.SuccessResponse(null, "Rezervasyon başarıyla güncellendi.");
        return Ok(response);
    }

    // DELETE: api/reservations/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _reservationService.DeleteReservationAsync(id);
        return NoContent(); // 204 No Content
    }
}

