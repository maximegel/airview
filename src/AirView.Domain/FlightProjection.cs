using System;
using AirView.Domain.Core;

namespace AirView.Domain
{
    public class FlightProjection : Entity<Guid>
    {
        public FlightProjection(Guid id) :
            base(id)
        {
        }

        public DateTime ArrivalTime { get; set; }

        public DateTime DepartureTime { get; set; }

        public string Number { get; set; }
    }
}