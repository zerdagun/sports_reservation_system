namespace sports_reservation_system.Business.DTOs.BranchDtos;

/// <summary>
/// Şube bilgilerini döndürmek için DTO
/// </summary>
public class BranchDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}