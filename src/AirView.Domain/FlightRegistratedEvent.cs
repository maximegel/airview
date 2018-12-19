using AirView.Domain.Core;

namespace AirView.Domain
{
    public class FlightRegistratedEvent : AggregateEvent
    {
        public FlightRegistratedEvent(string number) =>
            Number = number;

        public string Number { get; }
    }
}