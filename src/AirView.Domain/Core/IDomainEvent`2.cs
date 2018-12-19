namespace AirView.Domain.Core
{
    public interface IDomainEvent<TAggregate, out TAggregateEvent> :
        IDomainEvent<TAggregate>
        where TAggregate : IAggregateRoot
        where TAggregateEvent : IAggregateEvent
    {
        new TAggregateEvent Data { get; }
    }
}