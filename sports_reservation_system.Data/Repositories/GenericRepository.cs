using Microsoft.EntityFrameworkCore;
using sports_reservation_system.Data.Entities;
using System.Linq.Expressions;

namespace sports_reservation_system.Data.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    // Veritabanı bağlamını (Context) buraya alıyoruz
    protected readonly AppDbContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
    {
        // Soft Delete: Sadece silinmemiş kayıtları kontrol et
        return await _dbSet.Where(x => !x.IsDeleted).AnyAsync(expression);
    }

    public IQueryable<T> GetAll()
    {
        // Soft Delete: Sadece silinmemiş kayıtları getir (IsDeleted = false)
        // AsNoTracking: Sadece okuma yaparken performansı artırır
        return _dbSet.Where(x => !x.IsDeleted).AsNoTracking();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        // Soft Delete: Silinmemiş kayıtları getir
        return await _dbSet.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public void Remove(T entity)
    {
        // Soft Delete: Fiziksel silme yerine IsDeleted = true yap
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _dbSet.Update(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public IQueryable<T> Where(Expression<Func<T, bool>> expression)
    {
        // Soft Delete: Sadece silinmemiş kayıtları filtrele
        return _dbSet.Where(x => !x.IsDeleted).Where(expression);
    }
}