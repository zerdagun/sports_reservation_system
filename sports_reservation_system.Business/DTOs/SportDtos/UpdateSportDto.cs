namespace sports_reservation_system.Business.DTOs.SportDtos;

public class UpdateSportDto
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}
