namespace sports_reservation_system.Business.DTOs.AuthDtos;

public class RegisterDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; } // Şifreyi burada alacağız ama veritabanına hashleyip kaydedeceğiz
    public string? PhoneNumber { get; set; }
}