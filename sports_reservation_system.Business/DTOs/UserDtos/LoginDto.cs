using System.ComponentModel.DataAnnotations;

namespace sports_reservation_system.Business.DTOs.UserDtos;

/// <summary>
/// Kullanıcı girişi için DTO (JWT Authentication için)
/// </summary>
public class LoginDto
{
    [Required(ErrorMessage = "Email zorunludur")]
    [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Şifre zorunludur")]
    public required string Password { get; set; }
}

