using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sports_reservation_system.Business.Common;
using sports_reservation_system.Business.DTOs.SportDtos;
using sports_reservation_system.Business.Services;

namespace sports_reservation_system.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SportsController : ControllerBase
{
    private readonly ISportService _sportService;
    private readonly ILogger<SportsController> _logger;

    public SportsController(ISportService sportService, ILogger<SportsController> logger)
    {
        _sportService = sportService;
        _logger = logger;
    }

    /// <summary>
    /// Tüm sporları getirir
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("GET /api/sports - Tüm sporlar getiriliyor");
        var sports = await _sportService.GetAllSportsAsync();
        return Ok(ApiResponse<IEnumerable<SportDto>>.SuccessResponse(sports, "Sporlar başarıyla getirildi."));
    }

    /// <summary>
    /// ID'ye göre spor getirir
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("GET /api/sports/{Id} - Spor getiriliyor", id);
        var sport = await _sportService.GetSportByIdAsync(id);
        
        if (sport == null)
        {
            _logger.LogWarning("Spor bulunamadı: {Id}", id);
            return NotFound(ApiResponse<SportDto>.ErrorResponse($"ID'si {id} olan spor bulunamadı."));
        }

        return Ok(ApiResponse<SportDto>.SuccessResponse(sport, "Spor başarıyla getirildi."));
    }

    /// <summary>
    /// Yeni spor ekler (Admin yetkisi gerekir)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateSportDto dto)
    {
        _logger.LogInformation("POST /api/sports - Yeni spor ekleniyor: {Name}", dto.Name);
        var sport = await _sportService.AddSportAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = sport.Id }, 
            ApiResponse<SportDto>.SuccessResponse(sport, "Spor başarıyla eklendi."));
    }

    /// <summary>
    /// Spor günceller (Admin yetkisi gerekir)
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSportDto dto)
    {
        _logger.LogInformation("PUT /api/sports/{Id} - Spor güncelleniyor", id);
        
        try
        {
            await _sportService.UpdateSportAsync(id, dto);
            return Ok(ApiResponse<object>.SuccessResponse(null, "Spor başarıyla güncellendi."));
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning("Spor güncellenirken hata: {Message}", ex.Message);
            return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }

    /// <summary>
    /// Spor siler - Soft Delete (Admin yetkisi gerekir)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("DELETE /api/sports/{Id} - Spor siliniyor", id);
        
        try
        {
            await _sportService.DeleteSportAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning("Spor silinirken hata: {Message}", ex.Message);
            return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }
}
