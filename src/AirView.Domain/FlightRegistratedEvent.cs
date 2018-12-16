using System;
using AirView.Domain.Core;

namespace AirView.Domain
{
    public class FlightRegistratedEvent : 
        IAggregateEvent<Flight, Guid>
    {
        public FlightRegistratedEvent(string number) => 
            Number = number;

        public string Number { get; }
    }
}