using sports_reservation_system.Business.DTOs.SessionDtos;

namespace sports_reservation_system.Business.Services;

public interface ISessionService
{
    Task<IEnumerable<SessionDto>> GetAllSessionsAsync();
    Task<SessionDto?> GetSessionByIdAsync(int id);
    Task<SessionDto> AddSessionAsync(CreateSessionDto sessionDto);
    Task UpdateSessionAsync(int id, UpdateSessionDto sessionDto);
    Task DeleteSessionAsync(int id);
}

