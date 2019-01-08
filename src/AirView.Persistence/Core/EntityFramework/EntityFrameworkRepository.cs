using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AirView.Domain.Core;
using AirView.Shared.Railways;
using Microsoft.EntityFrameworkCore;

namespace AirView.Persistence.Core.EntityFramework
{
    public class EntityFrameworkRepository<TEntity> :
        IQueryableRepository<TEntity>,
        IWritableRepository<TEntity>
        where TEntity : class, IEntity
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _set;

        public EntityFrameworkRepository(DbContext dbContext)
        {
            _context = dbContext;
            _set = _context.Set<TEntity>();
        }

        public IQueryable<TEntity> Query() =>
            _set.AsNoTracking();

        Task<Option<TEntity>> IReadableRepository<TEntity>.TryFindAsync(
            object id, CancellationToken cancellationToken) =>
            ExecuteAsNoTracking(() => ((IWritableRepository<TEntity>) this).TryFindAsync(id, cancellationToken));

        public void Add(TEntity entity) =>
            _set.Add(entity);

        public void Attach(TEntity entity) =>
            _context.Attach(entity).State = EntityState.Modified;

        public void Remove(TEntity entity) =>
            _set.Remove(entity);

        public Task SaveAsync(CancellationToken cancellationToken) =>
            _context.SaveChangesAsync(cancellationToken);

        async Task<Option<TEntity>> IWritableRepository<TEntity>.TryFindAsync(
            object id, CancellationToken cancellationToken) =>
            Option.From(await _set.FindAsync(new object[] {id}, cancellationToken));

        private TResult ExecuteAsNoTracking<TResult>(Func<TResult> query)
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var result = query();
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
            return result;
        }
    }
}