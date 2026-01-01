using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using sports_reservation_system.Business.DTOs.SportDtos;
using sports_reservation_system.Data.Entities;
using sports_reservation_system.Data.Repositories;
using sports_reservation_system.Data.UnitOfWork;

namespace sports_reservation_system.Business.Services;

public class SportManager : ISportService
{
    private readonly IGenericRepository<Sport> _sportRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<SportManager> _logger;

    public SportManager(
        IGenericRepository<Sport> sportRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<SportManager> logger)
    {
        _sportRepository = sportRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<SportDto>> GetAllSportsAsync()
    {
        _logger.LogInformation("Tüm sporlar getiriliyor...");
        var sports = await _sportRepository.GetAll()
            .Where(s => !s.IsDeleted)
            .ToListAsync();
        return _mapper.Map<IEnumerable<SportDto>>(sports);
    }

    public async Task<SportDto?> GetSportByIdAsync(int id)
    {
        _logger.LogInformation("Spor getiriliyor: {SportId}", id);
        var sport = await _sportRepository.GetByIdAsync(id);
        
        if (sport == null || sport.IsDeleted)
        {
            _logger.LogWarning("ID'si {SportId} olan spor bulunamadı", id);
            return null;
        }

        return _mapper.Map<SportDto>(sport);
    }

    public async Task<SportDto> AddSportAsync(CreateSportDto dto)
    {
        _logger.LogInformation("Yeni spor ekleniyor: {SportName}", dto.Name);
        
        var sport = _mapper.Map<Sport>(dto);
        sport.CreatedAt = DateTime.UtcNow;
        
        await _sportRepository.AddAsync(sport);
        await _unitOfWork.CommitAsync();
        
        _logger.LogInformation("Spor başarıyla eklendi: {SportId}", sport.Id);
        return _mapper.Map<SportDto>(sport);
    }

    public async Task UpdateSportAsync(int id, UpdateSportDto dto)
    {
        _logger.LogInformation("Spor güncelleniyor: {SportId}", id);
        
        var sport = await _sportRepository.GetByIdAsync(id);
        if (sport == null || sport.IsDeleted)
        {
            _logger.LogWarning("ID'si {SportId} olan spor bulunamadı", id);
            throw new KeyNotFoundException($"ID'si {id} olan spor bulunamadı.");
        }

        _mapper.Map(dto, sport);
        sport.UpdatedAt = DateTime.UtcNow;
        
        _sportRepository.Update(sport);
        await _unitOfWork.CommitAsync();
        
        _logger.LogInformation("Spor başarıyla güncellendi: {SportId}", id);
    }

    public async Task DeleteSportAsync(int id)
    {
        _logger.LogInformation("Spor siliniyor (soft delete): {SportId}", id);
        
        var sport = await _sportRepository.GetByIdAsync(id);
        if (sport == null)
        {
            _logger.LogWarning("ID'si {SportId} olan spor bulunamadı", id);
            throw new KeyNotFoundException($"ID'si {id} olan spor bulunamadı.");
        }

        // Soft delete
        _sportRepository.Remove(sport);
        await _unitOfWork.CommitAsync();
        
        _logger.LogInformation("Spor başarıyla silindi (soft delete): {SportId}", id);
    }
}
