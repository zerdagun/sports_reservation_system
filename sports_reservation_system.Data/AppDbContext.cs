using Microsoft.EntityFrameworkCore;
using sports_reservation_system.Data.Entities;

namespace sports_reservation_system.Data;

public class AppDbContext : DbContext
{
    // Constructor: Ayarları (hangi veritabanı vs.) dışarıdan alabilmek için.
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Tabloları Veritabanına Tanıtıyoruz
    public DbSet<User> Users { get; set; }
    public DbSet<Branch> Branches { get; set; }
    public DbSet<Sport> Sports { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<Reservation> Reservations { get; set; }

    // Ayarlar ve İlişkiler (Fluent API)
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Reservation tablosunda UserId ve SessionId üzerinden ilişki kuralım
        // Aslında EF Core bunu otomatik anlar ama garanti olsun diye açıkça yazıyoruz.
        
        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.User)
            .WithMany(u => u.Reservations)
            .HasForeignKey(r => r.UserId);

        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Session)
            .WithMany(s => s.Reservations)
            .HasForeignKey(r => r.SessionId);

        // Session - Branch relationship
        modelBuilder.Entity<Session>()
            .HasOne(s => s.Branch)
            .WithMany(b => b.Sessions)
            .HasForeignKey(s => s.BranchId);

        // Session - Sport relationship
        modelBuilder.Entity<Session>()
            .HasOne(s => s.Sport)
            .WithMany(sp => sp.Sessions)
            .HasForeignKey(s => s.SportId);

        base.OnModelCreating(modelBuilder);
    }
}