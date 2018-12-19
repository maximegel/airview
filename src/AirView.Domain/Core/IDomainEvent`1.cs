namespace AirView.Domain.Core
{
    public interface IDomainEvent<TAggregate> :
        IDomainEvent
        where TAggregate : IAggregateRoot
    {
    }
}