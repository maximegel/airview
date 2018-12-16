using System.Linq;
using AirView.Domain.Core;

namespace AirView.Persistence.Core
{
    public interface IQueryableRepository<in TId, TEntity> :
        IReadableRepository<TId, TEntity>
        where TEntity : IEntity<TId>
    {
        IQueryable<TEntity> QueryAll();
    }
}