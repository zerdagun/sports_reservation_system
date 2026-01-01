using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using sports_reservation_system.Business.DTOs.UserDtos;
using sports_reservation_system.Data.Entities;
using sports_reservation_system.Data.Repositories;
using sports_reservation_system.Data.UnitOfWork;

namespace sports_reservation_system.Business.Services;

public class UserManager : IUserService
{
    private readonly IGenericRepository<User> _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public UserManager(
        IGenericRepository<User> userRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAll().ToListAsync();
        return _mapper.Map<IEnumerable<UserDto>>(users);
    }

    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            return null;

        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> AddUserAsync(CreateUserDto userDto)
    {
        // Email kontrolü
        var existingUser = await _userRepository.Where(u => u.Email == userDto.Email).FirstOrDefaultAsync();
        if (existingUser != null)
            throw new InvalidOperationException("Bu email adresi zaten kullanılıyor.");

        // Password hash'leme
        var passwordHash = HashPassword(userDto.Password);

        // Entity oluştur
        var user = _mapper.Map<User>(userDto);
        user.PasswordHash = passwordHash;

        await _userRepository.AddAsync(user);
        await _unitOfWork.CommitAsync();

        return _mapper.Map<UserDto>(user);
    }

    public async Task UpdateUserAsync(int id, UpdateUserDto userDto)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            throw new KeyNotFoundException($"ID'si {id} olan kullanıcı bulunamadı.");

        // Email değişiyorsa kontrol et
        if (user.Email != userDto.Email)
        {
            var existingUser = await _userRepository.Where(u => u.Email == userDto.Email).FirstOrDefaultAsync();
            if (existingUser != null)
                throw new InvalidOperationException("Bu email adresi zaten kullanılıyor.");
        }

        _mapper.Map(userDto, user);
        user.UpdatedAt = DateTime.UtcNow;

        _userRepository.Update(user);
        await _unitOfWork.CommitAsync();
    }

    public async Task DeleteUserAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            throw new KeyNotFoundException($"ID'si {id} olan kullanıcı bulunamadı.");

        _userRepository.Remove(user);
        await _unitOfWork.CommitAsync();
    }

    public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
    {
        // Kullanıcıyı email ile bul
        var user = await _userRepository.Where(u => u.Email == loginDto.Email).FirstOrDefaultAsync();
        if (user == null)
            throw new UnauthorizedAccessException("Email veya şifre hatalı.");

        // Şifre kontrolü
        if (!VerifyPassword(loginDto.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Email veya şifre hatalı.");

        // JWT Token oluştur
        var token = GenerateJwtToken(user);

        return new LoginResponseDto
        {
            Token = token,
            User = _mapper.Map<UserDto>(user),
            ExpiresAt = DateTime.UtcNow.AddHours(24) // 24 saat geçerli
        };
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    private bool VerifyPassword(string password, string passwordHash)
    {
        var hashOfInput = HashPassword(password);
        return hashOfInput == passwordHash;
    }

    private string GenerateJwtToken(User user)
    {
        var jwtKey = _configuration["Jwt:Key"] ?? "YourSuperSecretKeyThatShouldBeAtLeast32CharactersLong!";
        var jwtIssuer = _configuration["Jwt:Issuer"] ?? "SportsReservationSystem";
        var jwtAudience = _configuration["Jwt:Audience"] ?? "SportsReservationSystem";

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

