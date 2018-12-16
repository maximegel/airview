using System;

namespace AirView.Api.Flights
{
    public class ScheduleFlightDto
    {
        public DateTime ArrivalTime { get; set; }

        public DateTime DepartureTime { get; set; }
    }
}