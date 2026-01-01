using sports_reservation_system.Business.DTOs.ReservationDtos;

namespace sports_reservation_system.Business.Services;

public interface IReservationService
{
    Task<IEnumerable<ReservationDto>> GetAllReservationsAsync();
    Task<ReservationDto?> GetReservationByIdAsync(int id);
    Task<ReservationDto> AddReservationAsync(CreateReservationDto reservationDto);
    Task UpdateReservationAsync(int id, UpdateReservationDto reservationDto);
    Task DeleteReservationAsync(int id);
}

