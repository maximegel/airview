namespace AirView.Domain.Core
{
    public class AggregateRemovedEvent :
        IAggregateEvent
    {
        private AggregateRemovedEvent()
        {
        }

        public static AggregateRemovedEvent Of(IAggregateRoot aggregate) =>
            new AggregateRemovedEvent();
    }
}