using System;
using AirView.Domain.Core;

namespace AirView.Domain
{
    public class FlightScheduledEvent : 
        IAggregateEvent<Flight, Guid>
    {
        public FlightScheduledEvent(DateTime departureTime, DateTime arrivalTime)
        {
            DepartureTime = departureTime;
            ArrivalTime = departureTime;
        }

        public DateTime ArrivalTime { get; }

        public DateTime DepartureTime { get; }
    }
}