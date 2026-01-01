using sports_reservation_system.Business.DTOs.SportDtos;

namespace sports_reservation_system.Business.Services;

public interface ISportService
{
    Task<IEnumerable<SportDto>> GetAllSportsAsync();
    Task<SportDto?> GetSportByIdAsync(int id);
    Task<SportDto> AddSportAsync(CreateSportDto dto);
    Task UpdateSportAsync(int id, UpdateSportDto dto);
    Task DeleteSportAsync(int id);
}
