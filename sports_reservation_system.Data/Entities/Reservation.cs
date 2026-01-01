namespace sports_reservation_system.Data.Entities;

public class Reservation : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int SessionId { get; set; }
    public Session Session { get; set; } = null!;
}