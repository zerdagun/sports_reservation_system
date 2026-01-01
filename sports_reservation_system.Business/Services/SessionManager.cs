using AutoMapper;
using Microsoft.EntityFrameworkCore;
using sports_reservation_system.Business.DTOs.SessionDtos;
using sports_reservation_system.Data.Entities;
using sports_reservation_system.Data.Repositories;
using sports_reservation_system.Data.UnitOfWork;

namespace sports_reservation_system.Business.Services;

public class SessionManager : ISessionService
{
    private readonly IGenericRepository<Session> _sessionRepository;
    private readonly IGenericRepository<Branch> _branchRepository;
    private readonly IGenericRepository<Sport> _sportRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SessionManager(
        IGenericRepository<Session> sessionRepository,
        IGenericRepository<Branch> branchRepository,
        IGenericRepository<Sport> sportRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _sessionRepository = sessionRepository;
        _branchRepository = branchRepository;
        _sportRepository = sportRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SessionDto>> GetAllSessionsAsync()
    {
        var sessions = await _sessionRepository.GetAll()
            .Include(s => s.Branch)
            .Include(s => s.Sport)
            .ToListAsync();

        return _mapper.Map<IEnumerable<SessionDto>>(sessions);
    }

    public async Task<SessionDto?> GetSessionByIdAsync(int id)
    {
        var session = await _sessionRepository.GetAll()
            .Include(s => s.Branch)
            .Include(s => s.Sport)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (session == null)
            return null;

        return _mapper.Map<SessionDto>(session);
    }

    public async Task<SessionDto> AddSessionAsync(CreateSessionDto sessionDto)
    {
        // Branch kontrolü
        var branch = await _branchRepository.GetByIdAsync(sessionDto.BranchId);
        if (branch == null)
            throw new KeyNotFoundException($"ID'si {sessionDto.BranchId} olan şube bulunamadı.");

        // Sport kontrolü
        var sport = await _sportRepository.GetByIdAsync(sessionDto.SportId);
        if (sport == null)
            throw new KeyNotFoundException($"ID'si {sessionDto.SportId} olan spor bulunamadı.");

        var session = _mapper.Map<Session>(sessionDto);
        session.CreatedAt = DateTime.UtcNow;
        await _sessionRepository.AddAsync(session);
        await _unitOfWork.CommitAsync();

        // Branch ve Sport bilgisini dahil ederek döndür
        session = await _sessionRepository.GetAll()
            .Include(s => s.Branch)
            .Include(s => s.Sport)
            .FirstOrDefaultAsync(s => s.Id == session.Id) ?? session;

        return _mapper.Map<SessionDto>(session);
    }

    public async Task UpdateSessionAsync(int id, UpdateSessionDto sessionDto)
    {
        var session = await _sessionRepository.GetByIdAsync(id);
        if (session == null)
            throw new KeyNotFoundException($"ID'si {id} olan seans bulunamadı.");

        _mapper.Map(sessionDto, session);
        session.UpdatedAt = DateTime.UtcNow;

        _sessionRepository.Update(session);
        await _unitOfWork.CommitAsync();
    }

    public async Task DeleteSessionAsync(int id)
    {
        var session = await _sessionRepository.GetByIdAsync(id);
        if (session == null)
            throw new KeyNotFoundException($"ID'si {id} olan seans bulunamadı.");

        _sessionRepository.Remove(session);
        await _unitOfWork.CommitAsync();
    }
}

