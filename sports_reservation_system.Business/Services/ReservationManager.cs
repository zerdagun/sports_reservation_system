using AutoMapper;
using Microsoft.EntityFrameworkCore;
using sports_reservation_system.Business.DTOs.ReservationDtos;
using sports_reservation_system.Data.Entities;
using sports_reservation_system.Data.Repositories;
using sports_reservation_system.Data.UnitOfWork;

namespace sports_reservation_system.Business.Services;

public class ReservationManager : IReservationService
{
    private readonly IGenericRepository<Reservation> _reservationRepository;
    private readonly IGenericRepository<Session> _sessionRepository;
    private readonly IGenericRepository<User> _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ReservationManager(
        IGenericRepository<Reservation> reservationRepository,
        IGenericRepository<Session> sessionRepository,
        IGenericRepository<User> userRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _reservationRepository = reservationRepository;
        _sessionRepository = sessionRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ReservationDto>> GetAllReservationsAsync()
    {
        var reservations = await _reservationRepository.GetAll()
            .Include(r => r.User)
            .Include(r => r.Session)
                .ThenInclude(s => s.Branch)
            .ToListAsync();

        return _mapper.Map<IEnumerable<ReservationDto>>(reservations);
    }

    public async Task<ReservationDto?> GetReservationByIdAsync(int id)
    {
        var reservation = await _reservationRepository.GetAll()
            .Include(r => r.User)
            .Include(r => r.Session)
                .ThenInclude(s => s.Branch)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (reservation == null)
            return null;

        return _mapper.Map<ReservationDto>(reservation);
    }

    public async Task<ReservationDto> AddReservationAsync(CreateReservationDto reservationDto)
    {
        // User kontrolü
        var user = await _userRepository.GetByIdAsync(reservationDto.UserId);
        if (user == null)
            throw new KeyNotFoundException($"ID'si {reservationDto.UserId} olan kullanıcı bulunamadı.");

        // Session kontrolü
        var session = await _sessionRepository.GetByIdAsync(reservationDto.SessionId);
        if (session == null)
            throw new KeyNotFoundException($"ID'si {reservationDto.SessionId} olan seans bulunamadı.");

        // Kota kontrolü
        var currentReservations = await _reservationRepository
            .Where(r => r.SessionId == reservationDto.SessionId)
            .CountAsync();

        if (currentReservations >= session.Quota)
            throw new InvalidOperationException("Seans kotası dolmuş. Rezervasyon yapılamaz.");

        // Aynı kullanıcının aynı seansa tekrar rezervasyon yapmasını engelle
        var existingReservation = await _reservationRepository
            .Where(r => r.UserId == reservationDto.UserId && r.SessionId == reservationDto.SessionId)
            .FirstOrDefaultAsync();

        if (existingReservation != null)
            throw new InvalidOperationException("Bu seans için zaten rezervasyonunuz bulunmaktadır.");

        var reservation = _mapper.Map<Reservation>(reservationDto);
        await _reservationRepository.AddAsync(reservation);
        await _unitOfWork.CommitAsync();

        // İlişkili verileri dahil ederek döndür
        reservation = await _reservationRepository.GetAll()
            .Include(r => r.User)
            .Include(r => r.Session)
                .ThenInclude(s => s.Branch)
            .FirstOrDefaultAsync(r => r.Id == reservation.Id) ?? reservation;

        return _mapper.Map<ReservationDto>(reservation);
    }

    public async Task UpdateReservationAsync(int id, UpdateReservationDto reservationDto)
    {
        var reservation = await _reservationRepository.GetByIdAsync(id);
        if (reservation == null)
            throw new KeyNotFoundException($"ID'si {id} olan rezervasyon bulunamadı.");

        // Yeni session kontrolü
        var session = await _sessionRepository.GetByIdAsync(reservationDto.SessionId);
        if (session == null)
            throw new KeyNotFoundException($"ID'si {reservationDto.SessionId} olan seans bulunamadı.");

        // Kota kontrolü
        var currentReservations = await _reservationRepository
            .Where(r => r.SessionId == reservationDto.SessionId && r.Id != id)
            .CountAsync();

        if (currentReservations >= session.Quota)
            throw new InvalidOperationException("Seans kotası dolmuş. Rezervasyon güncellenemez.");

        _mapper.Map(reservationDto, reservation);
        reservation.UpdatedAt = DateTime.UtcNow;

        _reservationRepository.Update(reservation);
        await _unitOfWork.CommitAsync();
    }

    public async Task DeleteReservationAsync(int id)
    {
        var reservation = await _reservationRepository.GetByIdAsync(id);
        if (reservation == null)
            throw new KeyNotFoundException($"ID'si {id} olan rezervasyon bulunamadı.");

        _reservationRepository.Remove(reservation);
        await _unitOfWork.CommitAsync();
    }
}

