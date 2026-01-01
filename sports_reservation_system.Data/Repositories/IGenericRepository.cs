using System.Linq.Expressions;
using sports_reservation_system.Data.Entities;

namespace sports_reservation_system.Data.Repositories;

public interface IGenericRepository<T> where T : BaseEntity
{
    // Veri Getirme İşlemleri
    IQueryable<T> GetAll(); 
    IQueryable<T> Where(Expression<Func<T, bool>> expression);
    Task<T?> GetByIdAsync(int id);
    Task<bool> AnyAsync(Expression<Func<T, bool>> expression);

    // Veri Değiştirme İşlemleri
    Task AddAsync(T entity);
    void Update(T entity);
    void Remove(T entity);
}