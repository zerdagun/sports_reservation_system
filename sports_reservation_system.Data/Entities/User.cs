namespace sports_reservation_system.Data.Entities;

public class User : BaseEntity
{
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public string Role { get; set; } = "User";

    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}