using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AirView.Domain.Core;
using AirView.Shared.Railways;
using Microsoft.EntityFrameworkCore;

namespace AirView.Persistence.Core.EntityFramework
{
    public class EntityFrameworkRepository<TId, TEntity, TDbContext> :
        IQueryableRepository<TId, TEntity>,
        IWritableRepository<TId, TEntity>
        where TEntity : class, IEntity<TId>
        where TDbContext : DbContext
    {
        private readonly TDbContext _context;
        private readonly DbSet<TEntity> _set;

        public EntityFrameworkRepository(TDbContext dbContext)
        {
            _context = dbContext;
            _set = _context.Set<TEntity>();
        }

        public IQueryable<TEntity> QueryAll() =>
            _set.AsNoTracking();

        Task<Option<TEntity>> IReadableRepository<TId, TEntity>.TryFindAsync(
            TId id, CancellationToken cancellationToken) =>
            ExecuteAsNoTracking(() => ((IWritableRepository<TId, TEntity>) this).TryFindAsync(id, cancellationToken));

        public void Add(TEntity entity) =>
            _set.Add(entity);

        public void Attach(TEntity entity) =>
            _context.Attach(entity).State = EntityState.Modified;

        public void Remove(TEntity entity) =>
            _set.Remove(entity);

        public Task SaveAsync(CancellationToken cancellationToken) =>
            _context.SaveChangesAsync(cancellationToken);

        async Task<Option<TEntity>> IWritableRepository<TId, TEntity>.TryFindAsync(
            TId id, CancellationToken cancellationToken) =>
            Option.From(await _set.FindAsync(id, cancellationToken));

        private TResult ExecuteAsNoTracking<TResult>(Func<TResult> query)
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var result = query();
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
            return result;
        }
    }
}