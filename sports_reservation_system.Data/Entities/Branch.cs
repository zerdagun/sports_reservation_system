namespace sports_reservation_system.Data.Entities;

public class Branch : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public ICollection<Session> Sessions { get; set; } = new List<Session>();
}