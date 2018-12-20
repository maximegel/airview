using System.Threading;
using System.Threading.Tasks;
using AirView.Domain.Core;
using AirView.Shared.Railways;

namespace AirView.Persistence.Core
{
    public interface IWritableRepository<TEntity> :
        IRepository<TEntity>
        where TEntity : IEntity
    {
        void Add(TEntity entity);

        void Attach(TEntity entity);

        void Remove(TEntity entity);

        Task SaveAsync(CancellationToken cancellationToken = default);

        Task<Option<TEntity>> TryFindAsync(object id, CancellationToken cancellationToken = default);
    }
}