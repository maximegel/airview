using System;
using AirView.Domain.Core;

namespace AirView.Domain
{
    public class FlightProjection : Projection<Guid>
    {
        public FlightProjection(Guid id) :
            base(id)
        {
        }

        private FlightProjection() :
            base(Guid.Empty)
        {
        }

        public DateTime ArrivalTime { get; private set; }

        public DateTime DepartureTime { get; private set; }

        public string Number { get; private set; }

        private void Apply(IDomainEvent<Flight, FlightRegistratedEvent> @event) =>
            Number = @event.Data.Number;

        private void Apply(IDomainEvent<Flight, FlightScheduledEvent> @event)
        {
            DepartureTime = @event.Data.DepartureTime;
            ArrivalTime = @event.Data.DepartureTime;
        }
    }
}