namespace sports_reservation_system.Data.Entities;

/// <summary>
/// Sport entity representing different sports available in the system
/// Examples: Ice Skating (Buz Pateni), Football, Basketball, etc.
/// </summary>
public class Sport : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation property
    public ICollection<Session> Sessions { get; set; } = new List<Session>();
}
