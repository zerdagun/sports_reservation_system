using System.ComponentModel.DataAnnotations;

namespace sports_reservation_system.Business.DTOs.ReservationDtos;

/// <summary>
/// Rezervasyon oluşturma için DTO
/// </summary>
public class CreateReservationDto
{
    [Required(ErrorMessage = "Kullanıcı ID zorunludur")]
    public int UserId { get; set; }

    [Required(ErrorMessage = "Seans ID zorunludur")]
    public int SessionId { get; set; }
}

