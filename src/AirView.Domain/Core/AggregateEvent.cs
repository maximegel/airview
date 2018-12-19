namespace AirView.Domain.Core
{
    public abstract class AggregateEvent :
        IAggregateEvent
    {
        protected AggregateEvent() =>
            Name = GetType().Name.Replace("Event", "");

        protected virtual string Name { get; }

        string IAggregateEvent.Name => Name;
    }
}