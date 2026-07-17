using HomeFlow.Domain.Interfaces;
using HomeFlow.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HomeFlow.Infrastructure.Repositories;

public sealed class Repository<T>(HomeFlowDbContext context) : IRepository<T> where T : class
{
    private readonly DbSet<T> _set = context.Set<T>();

    public async Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        await _set.FindAsync([id], cancellationToken) ?? throw new KeyNotFoundException($"{typeof(T).Name} no encontrado.");

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _set.ToListAsync(cancellationToken);

    public async Task<IEnumerable<T>> FindAsync(Func<T, bool> predicate, CancellationToken cancellationToken = default) =>
        (await _set.ToListAsync(cancellationToken)).Where(predicate);

    public async Task<T> FirstOrDefaultAsync(Func<T, bool> predicate, CancellationToken cancellationToken = default) =>
        (await _set.ToListAsync(cancellationToken)).FirstOrDefault(predicate)!;

    public async Task<bool> AnyAsync(Func<T, bool> predicate, CancellationToken cancellationToken = default) =>
        (await _set.ToListAsync(cancellationToken)).Any(predicate);

    public Task<int> CountAsync(CancellationToken cancellationToken = default) => _set.CountAsync(cancellationToken);
    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default) { await _set.AddAsync(entity, cancellationToken); return entity; }
    public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default) { await _set.AddRangeAsync(entities, cancellationToken); return entities; }
    public Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default) { _set.Update(entity); return Task.FromResult(entity); }
    public Task<T> DeleteAsync(T entity, CancellationToken cancellationToken = default) { _set.Remove(entity); return Task.FromResult(entity); }
    public async Task<bool> DeleteByIdAsync(int id, CancellationToken cancellationToken = default) { var entity = await _set.FindAsync([id], cancellationToken); if (entity is null) return false; _set.Remove(entity); return true; }
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => context.SaveChangesAsync(cancellationToken);
}

public sealed class UnitOfWork(HomeFlowDbContext context) : IUnitOfWork
{
    public IRepository<T> Repository<T>() where T : class => new Repository<T>(context);
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => context.SaveChangesAsync(cancellationToken);
    public Task BeginTransactionAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
    public Task CommitAsync(CancellationToken cancellationToken = default) => context.SaveChangesAsync(cancellationToken);
    public Task RollbackAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
    public void Dispose() => context.Dispose();
}
