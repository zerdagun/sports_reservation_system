using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sports_reservation_system.Business.Common;
using sports_reservation_system.Business.DTOs.BranchDtos;
using sports_reservation_system.Business.Services;

namespace sports_reservation_system.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize] // JWT Authentication gerekli
public class BranchesController : ControllerBase
{
    private readonly IBranchService _branchService;

    public BranchesController(IBranchService branchService)
    {
        _branchService = branchService;
    }

    // GET: api/branches
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var branches = await _branchService.GetAllBranchesAsync();
        var response = ApiResponse<IEnumerable<BranchDto>>.SuccessResponse(branches, "Şubeler başarıyla getirildi.");
        return Ok(response);
    }

    // GET: api/branches/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var branch = await _branchService.GetBranchByIdAsync(id);
        if (branch == null)
        {
            var errorResponse = ApiResponse<BranchDto>.ErrorResponse($"ID'si {id} olan şube bulunamadı.");
            return NotFound(errorResponse);
        }

        var response = ApiResponse<BranchDto>.SuccessResponse(branch, "Şube başarıyla getirildi.");
        return Ok(response);
    }

    // POST: api/branches
    [HttpPost]
    [Authorize(Roles = "Admin")] // Sadece Admin ekleyebilir
    public async Task<IActionResult> Add([FromBody] CreateBranchDto branchDto)
    {
        await _branchService.AddBranchAsync(branchDto);
        var response = ApiResponse<object>.SuccessResponse(null, "Şube başarıyla eklendi.");
        return StatusCode(201, response);
    }

    // PUT: api/branches/{id}
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")] // Sadece Admin güncelleyebilir
    public async Task<IActionResult> Update(int id, [FromBody] UpdateBranchDto branchDto)
    {
        await _branchService.UpdateBranchAsync(id, branchDto);
        var response = ApiResponse<object>.SuccessResponse(null, "Şube başarıyla güncellendi.");
        return Ok(response);
    }

    // DELETE: api/branches/{id}
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")] // Sadece Admin silebilir
    public async Task<IActionResult> Delete(int id)
    {
        await _branchService.DeleteBranchAsync(id);
        return NoContent(); // 204 No Content
    }
}