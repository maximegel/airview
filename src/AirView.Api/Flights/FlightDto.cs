using System;
using System.ComponentModel;

namespace AirView.Api.Flights
{
    public class FlightDto
    {
        public Guid Id { get; set; }

        public string Number { get; set; }

        public DateTime ArrivalTime { get; set; }

        public DateTime DepartureTime { get; set; }
    }
}