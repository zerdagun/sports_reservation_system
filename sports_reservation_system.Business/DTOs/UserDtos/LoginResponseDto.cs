namespace sports_reservation_system.Business.DTOs.UserDtos;

/// <summary>
/// Login işlemi sonrası döndürülecek DTO (JWT Token ile)
/// </summary>
public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;
    public UserDto User { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
}

