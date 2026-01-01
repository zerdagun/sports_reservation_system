using System.ComponentModel.DataAnnotations;

namespace sports_reservation_system.Business.DTOs.UserDtos;

/// <summary>
/// Kullanıcı güncelleme için DTO
/// </summary>
public class UpdateUserDto
{
    [Required(ErrorMessage = "Ad Soyad zorunludur")]
    [StringLength(100, ErrorMessage = "Ad Soyad en fazla 100 karakter olabilir")]
    public required string FullName { get; set; }

    [Required(ErrorMessage = "Email zorunludur")]
    [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
    public required string Email { get; set; }

    public string? Role { get; set; } // Optional: Role değiştirilebilir
}

