using System.Threading;
using System.Threading.Tasks;
using AirView.Domain.Core;
using AirView.Shared.Railways;

namespace AirView.Persistence.Core
{
    public interface IWritableRepository<in TId, TEntity> :
        IRepository<TId, TEntity>
        where TEntity : IEntity<TId>
    {
        void Add(TEntity entity);

        void Attach(TEntity entity);

        void Remove(TEntity entity);

        Task SaveAsync(CancellationToken cancellationToken = default);

        Task<Option<TEntity>> TryFindAsync(TId id, CancellationToken cancellationToken = default);
    }
}