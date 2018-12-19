namespace AirView.Domain.Core
{
    public class AggregateRemovedEvent :
        IAggregateEvent
    {
        private AggregateRemovedEvent(string name) =>
            Name = name;

        private AggregateRemovedEvent()
        {
        }

        protected string Name { get; }

        string IAggregateEvent.Name => Name;

        public static AggregateRemovedEvent Of(IAggregateRoot aggregate) =>
            new AggregateRemovedEvent($"{aggregate.Name}Removed");
    }
}