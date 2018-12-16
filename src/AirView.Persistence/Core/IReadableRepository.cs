using System.Threading;
using System.Threading.Tasks;
using AirView.Domain.Core;
using AirView.Shared.Railways;

namespace AirView.Persistence.Core
{
    public interface IReadableRepository<in TId, TEntity> :
        IRepository<TId, TEntity>
        where TEntity : IEntity<TId>
    {
        Task<Option<TEntity>> TryFindAsync(TId id, CancellationToken cancellationToken = default);
    }
}