using AutoMapper;
using Microsoft.EntityFrameworkCore;
using sports_reservation_system.Business.DTOs.BranchDtos;
using sports_reservation_system.Data.Entities;
using sports_reservation_system.Data.Repositories;
using sports_reservation_system.Data.UnitOfWork;

namespace sports_reservation_system.Business.Services;

public class BranchManager : IBranchService
{
    private readonly IGenericRepository<Branch> _branchRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BranchManager(IGenericRepository<Branch> branchRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _branchRepository = branchRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task AddBranchAsync(CreateBranchDto branchDto)
    {
        // 1. DTO'yu Entity'ye çevir (Mapping)
        var branchEntity = _mapper.Map<Branch>(branchDto);

        // 2. Veritabanına eklemesi için Repository'e ver
        await _branchRepository.AddAsync(branchEntity);

        // 3. Değişiklikleri kesin olarak kaydet (Save Changes)
        await _unitOfWork.CommitAsync();
    }

    public async Task<IEnumerable<BranchDto>> GetAllBranchesAsync()
    {
        // 1. Veritabanından tüm şubeleri çek (soft delete: sadece silinmemişler)
        var branches = await _branchRepository.GetAll().ToListAsync();

        // 2. Entity listesini DTO listesine çevirip döndür
        return _mapper.Map<IEnumerable<BranchDto>>(branches);
    }

    public async Task<BranchDto?> GetBranchByIdAsync(int id)
    {
        // 1. ID'ye göre şubeyi bul (soft delete kontrolü repository'de yapılıyor)
        var branch = await _branchRepository.GetByIdAsync(id);

        // 2. Bulunamadıysa null döndür
        if (branch == null)
            return null;

        // 3. Entity'yi DTO'ya çevir ve döndür
        return _mapper.Map<BranchDto>(branch);
    }

    public async Task UpdateBranchAsync(int id, UpdateBranchDto branchDto)
    {
        // 1. Güncellenecek şubeyi bul
        var branch = await _branchRepository.GetByIdAsync(id);
        if (branch == null)
            throw new KeyNotFoundException($"ID'si {id} olan şube bulunamadı.");

        // 2. DTO'daki değerleri entity'ye aktar
        _mapper.Map(branchDto, branch);
        branch.UpdatedAt = DateTime.UtcNow;

        // 3. Güncellemeyi kaydet
        _branchRepository.Update(branch);
        await _unitOfWork.CommitAsync();
    }

    public async Task DeleteBranchAsync(int id)
    {
        // 1. Silinecek şubeyi bul
        var branch = await _branchRepository.GetByIdAsync(id);
        if (branch == null)
            throw new KeyNotFoundException($"ID'si {id} olan şube bulunamadı.");

        // 2. Soft delete yap (fiziksel silme değil, IsDeleted = true)
        _branchRepository.Remove(branch);
        await _unitOfWork.CommitAsync();
    }
}