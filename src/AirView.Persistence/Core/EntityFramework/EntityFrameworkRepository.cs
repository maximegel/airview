using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AirView.Domain.Core;
using AirView.Shared.Railways;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace AirView.Persistence.Core.EntityFramework
{
    public class EntityFrameworkRepository<TEntity> :
        IQueryableRepository<TEntity>,
        IWritableRepository<TEntity>,
        IUnitOfWorkParticipant
        where TEntity : class, IEntity
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;
        private IDbContextTransaction _transaction;

        public EntityFrameworkRepository(DbContext dbContext, IUnitOfWorkContext unitOfWorkContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
            unitOfWorkContext.Enlist(this);
        }

        public void Dispose()
        {
        }

        public IQueryable<TEntity> Query() =>
            _dbSet.AsNoTracking();

        Task<Option<TEntity>> IReadableRepository<TEntity>.TryFindAsync(
            object id, CancellationToken cancellationToken) =>
            ExecuteAsNoTracking(() => ((IWritableRepository<TEntity>) this).TryFindAsync(id, cancellationToken));

        public void Commit() => _transaction.Commit();

        public void Prepare() =>
            _transaction = _dbContext.Database.CurrentTransaction ?? _dbContext.Database.BeginTransaction();

        public void Rollback() => _transaction.Rollback();

        public Task SaveAsync(CancellationToken cancellationToken) =>
            _dbContext.SaveChangesAsync(cancellationToken);

        public void Add(TEntity entity) =>
            _dbSet.Add(entity);

        public void Attach(TEntity entity) =>
            _dbContext.Attach(entity).State = EntityState.Modified;

        public void Remove(TEntity entity) =>
            _dbSet.Remove(entity);

        async Task<Option<TEntity>> IWritableRepository<TEntity>.TryFindAsync(
            object id, CancellationToken cancellationToken) =>
            Option.From(await _dbSet.FindAsync(new[] {id}, cancellationToken));

        private TResult ExecuteAsNoTracking<TResult>(Func<TResult> query)
        {
            _dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var result = query();
            _dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
            return result;
        }
    }
}