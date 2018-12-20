using System.Threading;
using System.Threading.Tasks;
using AirView.Domain.Core;
using AirView.Shared.Railways;

namespace AirView.Persistence.Core
{
    public interface IReadableRepository<TEntity> :
        IRepository<TEntity>
        where TEntity : IEntity
    {
        Task<Option<TEntity>> TryFindAsync(object id, CancellationToken cancellationToken = default);
    }
}