using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using sports_reservation_system.Data;
using sports_reservation_system.Data.Repositories;
using sports_reservation_system.Data.UnitOfWork;
using sports_reservation_system.Data.Seed;
using sports_reservation_system.Business.Services;
using sports_reservation_system.Business.Mappings;
using sports_reservation_system.Business.Common;
using sports_reservation_system.Business.DTOs.BranchDtos;
using sports_reservation_system.Business.DTOs.UserDtos;
using sports_reservation_system.Business.DTOs.SessionDtos;
using sports_reservation_system.Business.DTOs.ReservationDtos;
using sports_reservation_system.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// --- 1. Veritabanı Ayarı ---
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- 2. Data Katmanı Servisleri ---
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// --- 3. Business Katmanı Servisleri ---
builder.Services.AddScoped<IBranchService, BranchManager>();
builder.Services.AddScoped<IUserService, UserManager>();
builder.Services.AddScoped<ISessionService, SessionManager>();
builder.Services.AddScoped<IReservationService, ReservationManager>();

// --- 4. AutoMapper Ayarı ---
builder.Services.AddAutoMapper(typeof(MappingProfile));

// --- 5. JWT Authentication ---
var jwtKey = builder.Configuration["Jwt:Key"] ?? "YourSuperSecretKeyThatShouldBeAtLeast32CharactersLongForSecurity!";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "SportsReservationSystem";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "SportsReservationSystem";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddAuthorization();

// --- 6. Swagger ve API Standartları ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --- Seed Data (Uygulama başlarken bir kez çalışır) ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    var logger = services.GetRequiredService<ILogger<Program>>();
    
    try
    {
        // Migration'ları uygula
        await context.Database.MigrateAsync();
        
        // Seed data ekle
        await DataSeeder.SeedAsync(context, logger);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Veritabanı migration veya seed işlemi sırasında hata oluştu.");
    }
}

// --- 6. Global Exception Handling Middleware (EN ÖNCE EKLENMELİ) ---
app.UseMiddleware<ExceptionHandlingMiddleware>();

// --- 7. Swagger Ayarları (Development ve Production'da açık) ---
app.UseSwagger();
app.UseSwaggerUI(c => 
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Spor Rezervasyon Sistemi API v1");
});

app.UseHttpsRedirection();

// Authentication ve Authorization middleware'leri
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers(); // Controller'ları aktif et

// ========== MINIMAL API ENDPOINTS ==========
// Ödev gereksinimi: CRUD işlemleri hem Controller hem Minimal API tarafında olmalı

// Ana sayfa
app.MapGet("/", () => "Spor Rezervasyon Sistemi API Calisiyor! 🚀 (Dokumantasyon icin /swagger adresine gidin)");

// ========== BRANCHES MINIMAL API ==========
var branchesGroup = app.MapGroup("api/minimal/branches").WithTags("Branches (Minimal API)");
branchesGroup.MapGet("/", [Authorize] async (IBranchService service) =>
{
    var branches = await service.GetAllBranchesAsync();
    return Results.Ok(ApiResponse<IEnumerable<BranchDto>>.SuccessResponse(branches, "Şubeler başarıyla getirildi."));
});

branchesGroup.MapGet("/{id}", [Authorize] async (int id, IBranchService service) =>
{
    var branch = await service.GetBranchByIdAsync(id);
    if (branch == null)
        return Results.NotFound(ApiResponse<BranchDto>.ErrorResponse($"ID'si {id} olan şube bulunamadı."));
    return Results.Ok(ApiResponse<BranchDto>.SuccessResponse(branch, "Şube başarıyla getirildi."));
});

branchesGroup.MapPost("/", [Authorize(Roles = "Admin")] async (CreateBranchDto dto, IBranchService service) =>
{
    await service.AddBranchAsync(dto);
    return Results.Created($"/api/minimal/branches", ApiResponse<object>.SuccessResponse(null, "Şube başarıyla eklendi."));
});

branchesGroup.MapPut("/{id}", [Authorize(Roles = "Admin")] async (int id, UpdateBranchDto dto, IBranchService service) =>
{
    await service.UpdateBranchAsync(id, dto);
    return Results.Ok(ApiResponse<object>.SuccessResponse(null, "Şube başarıyla güncellendi."));
});

branchesGroup.MapDelete("/{id}", [Authorize(Roles = "Admin")] async (int id, IBranchService service) =>
{
    await service.DeleteBranchAsync(id);
    return Results.NoContent();
});

// ========== USERS MINIMAL API ==========
var usersGroup = app.MapGroup("api/minimal/users").WithTags("Users (Minimal API)");
usersGroup.MapGet("/", [Authorize(Roles = "Admin")] async (IUserService service) =>
{
    var users = await service.GetAllUsersAsync();
    return Results.Ok(ApiResponse<IEnumerable<UserDto>>.SuccessResponse(users, "Kullanıcılar başarıyla getirildi."));
});

usersGroup.MapGet("/{id}", [Authorize] async (int id, IUserService service) =>
{
    var user = await service.GetUserByIdAsync(id);
    if (user == null)
        return Results.NotFound(ApiResponse<UserDto>.ErrorResponse($"ID'si {id} olan kullanıcı bulunamadı."));
    return Results.Ok(ApiResponse<UserDto>.SuccessResponse(user, "Kullanıcı başarıyla getirildi."));
});

usersGroup.MapPut("/{id}", [Authorize] async (int id, UpdateUserDto dto, IUserService service) =>
{
    await service.UpdateUserAsync(id, dto);
    return Results.Ok(ApiResponse<object>.SuccessResponse(null, "Kullanıcı başarıyla güncellendi."));
});

usersGroup.MapDelete("/{id}", [Authorize(Roles = "Admin")] async (int id, IUserService service) =>
{
    await service.DeleteUserAsync(id);
    return Results.NoContent();
});

// ========== SESSIONS MINIMAL API ==========
var sessionsGroup = app.MapGroup("api/minimal/sessions").WithTags("Sessions (Minimal API)");
sessionsGroup.MapGet("/", [Authorize] async (ISessionService service) =>
{
    var sessions = await service.GetAllSessionsAsync();
    return Results.Ok(ApiResponse<IEnumerable<SessionDto>>.SuccessResponse(sessions, "Seanslar başarıyla getirildi."));
});

sessionsGroup.MapGet("/{id}", [Authorize] async (int id, ISessionService service) =>
{
    var session = await service.GetSessionByIdAsync(id);
    if (session == null)
        return Results.NotFound(ApiResponse<SessionDto>.ErrorResponse($"ID'si {id} olan seans bulunamadı."));
    return Results.Ok(ApiResponse<SessionDto>.SuccessResponse(session, "Seans başarıyla getirildi."));
});

sessionsGroup.MapPost("/", [Authorize(Roles = "Admin")] async (CreateSessionDto dto, ISessionService service) =>
{
    var session = await service.AddSessionAsync(dto);
    return Results.Created($"/api/minimal/sessions/{session.Id}", ApiResponse<SessionDto>.SuccessResponse(session, "Seans başarıyla eklendi."));
});

sessionsGroup.MapPut("/{id}", [Authorize(Roles = "Admin")] async (int id, UpdateSessionDto dto, ISessionService service) =>
{
    await service.UpdateSessionAsync(id, dto);
    return Results.Ok(ApiResponse<object>.SuccessResponse(null, "Seans başarıyla güncellendi."));
});

sessionsGroup.MapDelete("/{id}", [Authorize(Roles = "Admin")] async (int id, ISessionService service) =>
{
    await service.DeleteSessionAsync(id);
    return Results.NoContent();
});

// ========== RESERVATIONS MINIMAL API ==========
var reservationsGroup = app.MapGroup("api/minimal/reservations").WithTags("Reservations (Minimal API)");
reservationsGroup.MapGet("/", [Authorize(Roles = "Admin")] async (IReservationService service) =>
{
    var reservations = await service.GetAllReservationsAsync();
    return Results.Ok(ApiResponse<IEnumerable<ReservationDto>>.SuccessResponse(reservations, "Rezervasyonlar başarıyla getirildi."));
});

reservationsGroup.MapGet("/{id}", [Authorize] async (int id, IReservationService service) =>
{
    var reservation = await service.GetReservationByIdAsync(id);
    if (reservation == null)
        return Results.NotFound(ApiResponse<ReservationDto>.ErrorResponse($"ID'si {id} olan rezervasyon bulunamadı."));
    return Results.Ok(ApiResponse<ReservationDto>.SuccessResponse(reservation, "Rezervasyon başarıyla getirildi."));
});

reservationsGroup.MapPost("/", [Authorize] async (CreateReservationDto dto, IReservationService service) =>
{
    var reservation = await service.AddReservationAsync(dto);
    return Results.Created($"/api/minimal/reservations/{reservation.Id}", ApiResponse<ReservationDto>.SuccessResponse(reservation, "Rezervasyon başarıyla oluşturuldu."));
});

reservationsGroup.MapPut("/{id}", [Authorize] async (int id, UpdateReservationDto dto, IReservationService service) =>
{
    await service.UpdateReservationAsync(id, dto);
    return Results.Ok(ApiResponse<object>.SuccessResponse(null, "Rezervasyon başarıyla güncellendi."));
});

reservationsGroup.MapDelete("/{id}", [Authorize] async (int id, IReservationService service) =>
{
    await service.DeleteReservationAsync(id);
    return Results.NoContent();
});

app.Run();