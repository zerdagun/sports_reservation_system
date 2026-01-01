using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using sports_reservation_system.Data.Entities;
using System.Security.Cryptography;
using System.Text;

namespace sports_reservation_system.Data.Seed;

/// <summary>
/// Veritabanına başlangıç verileri eklemek için kullanılır
/// </summary>
public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext context, ILogger logger)
    {
        try
        {
            logger.LogInformation("Seed data işlemi başlatılıyor...");

            // Veritabanı oluşturulmuş mu kontrol et
            await context.Database.EnsureCreatedAsync();

            // Users seed
            if (!await context.Users.AnyAsync())
            {
                logger.LogInformation("Kullanıcılar ekleniyor...");
                var users = new List<User>
                {
                    new User
                    {
                        FullName = "Admin User",
                        Email = "admin@example.com",
                        PasswordHash = HashPassword("Admin123!"),
                        Role = "Admin",
                        CreatedAt = DateTime.UtcNow
                    },
                    new User
                    {
                        FullName = "Test User",
                        Email = "user@example.com",
                        PasswordHash = HashPassword("User123!"),
                        Role = "User",
                        CreatedAt = DateTime.UtcNow
                    },
                    new User
                    {
                        FullName = "John Doe",
                        Email = "john@example.com",
                        PasswordHash = HashPassword("John123!"),
                        Role = "User",
                        CreatedAt = DateTime.UtcNow
                    }
                };

                await context.Users.AddRangeAsync(users);
                await context.SaveChangesAsync();
                logger.LogInformation($"{users.Count} kullanıcı eklendi.");
            }

            // Branches seed
            if (!await context.Branches.AnyAsync())
            {
                logger.LogInformation("Şubeler ekleniyor...");
                var branches = new List<Branch>
                {
                    new Branch
                    {
                        Name = "Merkez Şube",
                        Description = "Ana spor tesisi",
                        CreatedAt = DateTime.UtcNow
                    },
                    new Branch
                    {
                        Name = "Kuzey Şubesi",
                        Description = "Kuzey bölge spor tesisi",
                        CreatedAt = DateTime.UtcNow
                    },
                    new Branch
                    {
                        Name = "Güney Şubesi",
                        Description = "Güney bölge spor tesisi",
                        CreatedAt = DateTime.UtcNow
                    }
                };

                await context.Branches.AddRangeAsync(branches);
                await context.SaveChangesAsync();
                logger.LogInformation($"{branches.Count} şube eklendi.");
            }

            // Sports seed (BUZ PATENİ dahil! ⛸️)
            if (!await context.Sports.AnyAsync())
            {
                logger.LogInformation("Sporlar ekleniyor...");
                var sports = new List<Sport>
                {
                    new Sport
                    {
                        Name = "Buz Pateni",
                        Description = "Buz üzerinde yapılan patinaj sporu",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Sport
                    {
                        Name = "Futbol",
                        Description = "Halı saha futbol",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Sport
                    {
                        Name = "Basketbol",
                        Description = "Kapalı saha basketbol",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Sport
                    {
                        Name = "Tenis",
                        Description = "Açık ve kapalı kort tenis",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Sport
                    {
                        Name = "Yüzme",
                        Description = "Kapalı havuz yüzme",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    }
                };

                await context.Sports.AddRangeAsync(sports);
                await context.SaveChangesAsync();
                logger.LogInformation($"{sports.Count} spor eklendi.");
            }

            // Sessions seed
            if (!await context.Sessions.AnyAsync())
            {
                logger.LogInformation("Seanslar ekleniyor...");
                var branches = await context.Branches.ToListAsync();
                var sports = await context.Sports.ToListAsync();
                
                if (branches.Any() && sports.Any())
                {
                    var sessions = new List<Session>();
                    var random = new Random();

                    // Her şube için farklı sporlardan seanslar oluştur
                    foreach (var branch in branches)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            // Rastgele bir spor seç
                            var randomSport = sports[random.Next(sports.Count)];
                            
                            sessions.Add(new Session
                            {
                                BranchId = branch.Id,
                                SportId = randomSport.Id,
                                StartTime = DateTime.UtcNow.AddDays(i + 1).AddHours(10 + i),
                                DurationMinutes = 60,
                                Quota = random.Next(10, 30),
                                Price = random.Next(50, 200),
                                CreatedAt = DateTime.UtcNow
                            });
                        }
                    }

                    await context.Sessions.AddRangeAsync(sessions);
                    await context.SaveChangesAsync();
                    logger.LogInformation($"{sessions.Count} seans eklendi.");
                }
            }

            // Reservations seed
            if (!await context.Reservations.AnyAsync())
            {
                logger.LogInformation("Rezervasyonlar ekleniyor...");
                var users = await context.Users.Where(u => u.Role == "User").ToListAsync();
                var sessions = await context.Sessions.Take(3).ToListAsync();

                if (users.Any() && sessions.Any())
                {
                    var reservations = new List<Reservation>();
                    for (int i = 0; i < Math.Min(users.Count, sessions.Count); i++)
                    {
                        reservations.Add(new Reservation
                        {
                            UserId = users[i].Id,
                            SessionId = sessions[i].Id,
                            CreatedAt = DateTime.UtcNow
                        });
                    }

                    await context.Reservations.AddRangeAsync(reservations);
                    await context.SaveChangesAsync();
                    logger.LogInformation($"{reservations.Count} rezervasyon eklendi.");
                }
            }

            logger.LogInformation("Seed data işlemi tamamlandı.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Seed data işlemi sırasında hata oluştu.");
            throw;
        }
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }
}

