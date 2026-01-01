using sports_reservation_system.Business.DTOs.AuthDtos;
using sports_reservation_system.Business.DTOs.UserDtos;

namespace sports_reservation_system.Business.Services;

public interface IAuthService
{
    Task<string> RegisterAsync(RegisterDto registerDto);
    Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
}