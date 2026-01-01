using sports_reservation_system.Business.DTOs.UserDtos;

namespace sports_reservation_system.Business.Services;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<UserDto?> GetUserByIdAsync(int id);
    Task<UserDto> AddUserAsync(CreateUserDto userDto);
    Task UpdateUserAsync(int id, UpdateUserDto userDto);
    Task DeleteUserAsync(int id);
    Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
}

