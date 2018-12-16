namespace AirView.Domain.Core
{
    public interface IDomainEvent<TAggregate, out TAggregateId, out TAggregateEvent> :
        IDomainEvent<TAggregate, TAggregateId>
        where TAggregate : IAggregateRoot<TAggregate, TAggregateId>
        where TAggregateEvent : IAggregateEvent<TAggregate, TAggregateId>
    {
        new TAggregateId AggregateId { get; }

        new TAggregateEvent Data { get; }
    }
}