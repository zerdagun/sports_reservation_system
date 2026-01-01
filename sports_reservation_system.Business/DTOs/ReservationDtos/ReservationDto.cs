namespace sports_reservation_system.Business.DTOs.ReservationDtos;

/// <summary>
/// Rezervasyon bilgilerini döndürmek için DTO
/// </summary>
public class ReservationDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string UserFullName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public int SessionId { get; set; }
    public DateTime SessionStartTime { get; set; }
    public int SessionDurationMinutes { get; set; }
    public decimal SessionPrice { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

