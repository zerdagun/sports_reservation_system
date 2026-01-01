namespace sports_reservation_system.Data.UnitOfWork;

public interface IUnitOfWork
{
    // Değişiklikleri veritabanına asıl kaydeden komut
    Task CommitAsync(); 
    void Commit();
}