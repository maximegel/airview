using System.Linq;
using AirView.Domain.Core;

namespace AirView.Persistence.Core
{
    public interface IQueryableRepository<TEntity> :
        IReadableRepository<TEntity>
        where TEntity : IEntity
    {
        IQueryable<TEntity> QueryAll();
    }
}