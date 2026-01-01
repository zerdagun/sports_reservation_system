using System.ComponentModel.DataAnnotations;

namespace sports_reservation_system.Business.DTOs.BranchDtos;

/// <summary>
/// Şube güncelleme için DTO
/// </summary>
public class UpdateBranchDto
{
    [Required(ErrorMessage = "Şube adı zorunludur")]
    [StringLength(100, ErrorMessage = "Şube adı en fazla 100 karakter olabilir")]
    public required string Name { get; set; }

    [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
    public string? Description { get; set; }
}

