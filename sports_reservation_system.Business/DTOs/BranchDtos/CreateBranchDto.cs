namespace sports_reservation_system.Business.DTOs.BranchDtos;

public class CreateBranchDto
{
    public required string Name { get; set; }
    public string? Description { get; set; }
}