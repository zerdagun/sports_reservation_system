namespace sports_reservation_system.Business.DTOs.UserDtos;

/// <summary>
/// Kullanıcı bilgilerini döndürmek için DTO (PasswordHash dahil edilmez)
/// </summary>
public class UserDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = "User";
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

