using AirView.Domain.Core;

namespace AirView.Persistence.Core.Internal
{
    public class AggregateNeverCreatedEvent : AggregateEvent
    {
        private AggregateNeverCreatedEvent()
        {
        }

        public static AggregateNeverCreatedEvent Instance =>
            new AggregateNeverCreatedEvent();
    }
}