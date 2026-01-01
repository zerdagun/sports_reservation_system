namespace sports_reservation_system.Data.Entities;

public class Session : BaseEntity
{
    public DateTime StartTime { get; set; }
    public int DurationMinutes { get; set; }
    public int Quota { get; set; }
    public decimal Price { get; set; }

    public int BranchId { get; set; }
    public Branch Branch { get; set; } = null!;
    
    public int SportId { get; set; }
    public Sport Sport { get; set; } = null!;
    
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}