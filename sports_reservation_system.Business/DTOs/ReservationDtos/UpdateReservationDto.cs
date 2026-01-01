using System.ComponentModel.DataAnnotations;

namespace sports_reservation_system.Business.DTOs.ReservationDtos;

/// <summary>
/// Rezervasyon güncelleme için DTO (Genelde rezervasyonlar güncellenmez ama yapıyı tamamlamak için)
/// </summary>
public class UpdateReservationDto
{
    [Required(ErrorMessage = "Seans ID zorunludur")]
    public int SessionId { get; set; }
}

