using AirView.Domain.Core.Internal;

namespace AirView.Domain.Core
{
    public class Projection<TId> : Entity<TId>,
        IProjection<TId>
    {
        private readonly IEventRouter<IDomainEvent> _router;

        public Projection(TId id) :
            base(id) =>
            _router = new MethodsEventRouter<IDomainEvent>(this, "^Apply$");


        public void ApplyEvent(IDomainEvent @event) =>
            _router.Dispatch(@event);
    }

    public interface IProjection :
        IEntity
    {
        void ApplyEvent(IDomainEvent @event);
    }

    public interface IProjection<out TId> :
        IProjection,
        IEntity<TId>
    {
    }
}