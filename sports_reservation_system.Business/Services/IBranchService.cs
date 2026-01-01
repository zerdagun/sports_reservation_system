using sports_reservation_system.Business.DTOs.BranchDtos;

namespace sports_reservation_system.Business.Services;

public interface IBranchService
{
    Task<IEnumerable<BranchDto>> GetAllBranchesAsync();
    Task<BranchDto?> GetBranchByIdAsync(int id);
    Task AddBranchAsync(CreateBranchDto branchDto);
    Task UpdateBranchAsync(int id, UpdateBranchDto branchDto);
    Task DeleteBranchAsync(int id);
}