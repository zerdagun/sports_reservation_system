using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using sports_reservation_system.Business.DTOs.AuthDtos;
using sports_reservation_system.Business.DTOs.UserDtos;
using sports_reservation_system.Data.Entities;
using sports_reservation_system.Data.Repositories;
using sports_reservation_system.Data.UnitOfWork;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace sports_reservation_system.Business.Services;

public class AuthManager : IAuthService
{
    private readonly IGenericRepository<User> _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public AuthManager(IGenericRepository<User> userRepository, IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<string> RegisterAsync(RegisterDto registerDto)
    {
        // 1. Bu email ile daha önce kayıt olunmuş mu kontrol et
        var userExists = await _userRepository.Where(u => u.Email == registerDto.Email).AnyAsync();
        if (userExists)
        {
            throw new Exception("Bu email adresi zaten kullaniliyor.");
        }

        // 2. DTO'yu Entity'ye çevir
        var user = _mapper.Map<User>(registerDto);

        // 3. Şifreyi Hashle (Güvenlik)
        user.PasswordHash = HashPassword(registerDto.Password);
        user.Role = "Customer"; // Varsayılan rol

        // 4. Kaydet
        await _userRepository.AddAsync(user);
        await _unitOfWork.CommitAsync();

        return "Kayıt başarılı!";
    }

    public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
    {
        // 1. Kullanıcıyı bul
        var user = await _userRepository.Where(u => u.Email == loginDto.Email).FirstOrDefaultAsync();
        
        // 2. Kullanıcı yoksa veya şifre yanlışsa hata fırlat
        if (user == null || HashPassword(loginDto.Password) != user.PasswordHash)
        {
            throw new Exception("Email veya şifre hatalı.");
        }

        // 3. JWT Token üret
        var token = GenerateJwtToken(user);

        // 4. Response dön
        return new LoginResponseDto
        {
            Token = token,
            User = _mapper.Map<UserDto>(user),
            ExpiresAt = DateTime.UtcNow.AddDays(1)
        };
    }

    private string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role), // Role-based authorization için önemli
            new Claim("FullName", user.FullName)
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // Basit Şifre Hashleme Fonksiyonu (SHA256)
    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }
}