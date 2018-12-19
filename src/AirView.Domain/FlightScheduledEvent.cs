using System;
using AirView.Domain.Core;

namespace AirView.Domain
{
    public class FlightScheduledEvent : AggregateEvent
    {
        public FlightScheduledEvent(DateTime departureTime, DateTime arrivalTime)
        {
            DepartureTime = departureTime;
            ArrivalTime = arrivalTime;
        }

        public DateTime ArrivalTime { get; }

        public DateTime DepartureTime { get; }
    }
}