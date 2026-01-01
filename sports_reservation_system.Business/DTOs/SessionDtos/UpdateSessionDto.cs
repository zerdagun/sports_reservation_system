using System.ComponentModel.DataAnnotations;

namespace sports_reservation_system.Business.DTOs.SessionDtos;

/// <summary>
/// Seans güncelleme için DTO
/// </summary>
public class UpdateSessionDto
{
    [Required(ErrorMessage = "Başlangıç zamanı zorunludur")]
    public DateTime StartTime { get; set; }

    [Required(ErrorMessage = "Süre zorunludur")]
    [Range(1, 1440, ErrorMessage = "Süre 1-1440 dakika arasında olmalıdır")]
    public int DurationMinutes { get; set; }

    [Required(ErrorMessage = "Kota zorunludur")]
    [Range(1, 1000, ErrorMessage = "Kota 1-1000 arasında olmalıdır")]
    public int Quota { get; set; }

    [Required(ErrorMessage = "Fiyat zorunludur")]
    [Range(0, double.MaxValue, ErrorMessage = "Fiyat 0 veya pozitif olmalıdır")]
    public decimal Price { get; set; }
}

