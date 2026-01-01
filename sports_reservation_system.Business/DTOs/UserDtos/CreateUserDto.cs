using System.ComponentModel.DataAnnotations;

namespace sports_reservation_system.Business.DTOs.UserDtos;

/// <summary>
/// Kullanıcı oluşturma için DTO
/// </summary>
public class CreateUserDto
{
    [Required(ErrorMessage = "Ad Soyad zorunludur")]
    [StringLength(100, ErrorMessage = "Ad Soyad en fazla 100 karakter olabilir")]
    public required string FullName { get; set; }

    [Required(ErrorMessage = "Email zorunludur")]
    [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Şifre zorunludur")]
    [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır")]
    public required string Password { get; set; }

    public string Role { get; set; } = "User"; // Default: User, Admin olabilir
}

