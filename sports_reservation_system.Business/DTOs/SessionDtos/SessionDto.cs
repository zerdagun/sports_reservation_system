namespace sports_reservation_system.Business.DTOs.SessionDtos;

/// <summary>
/// Seans bilgilerini döndürmek için DTO
/// </summary>
public class SessionDto
{
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public int DurationMinutes { get; set; }
    public int Quota { get; set; }
    public decimal Price { get; set; }
    public int BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

