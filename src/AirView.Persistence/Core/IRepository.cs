using AirView.Domain.Core;

namespace AirView.Persistence.Core
{
    public interface IRepository<in TId, out TEntity>
        where TEntity : IEntity<TId>
    {
    }
}