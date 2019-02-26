using System;
using AirView.Domain.Core;

namespace AirView.Domain
{
    public class Flight : AggregateRoot<Guid>
    {
        public Flight(Guid id, string number) :
            base(id) =>
            Raise(new FlightRegistratedEvent(number));

        public DateTime ArrivalTime { get; private set; }

        public DateTime DepartureTime { get; private set; }

        public string Number { get; private set; }

        public void Schedule(DateTime departureTime, DateTime arrivalTime) =>
            Raise(new FlightScheduledEvent(departureTime, arrivalTime));

        private void Apply(FlightRegistratedEvent @event) =>
            Number = @event.Number;

        private void Apply(FlightScheduledEvent @event)
        {
            DepartureTime = @event.DepartureTime;
            ArrivalTime = @event.ArrivalTime;
        }
    }
}