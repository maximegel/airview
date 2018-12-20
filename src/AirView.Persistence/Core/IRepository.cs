using AirView.Domain.Core;

namespace AirView.Persistence.Core
{
    public interface IRepository<out TEntity>
        where TEntity : IEntity
    {
    }
}