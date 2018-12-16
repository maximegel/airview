using System;
using AirView.Application.Core;

namespace AirView.Application
{
    public class ScheduleFlightCommand : Command<ScheduleFlightCommand>,
        IAccessOptionalEntityCommand
    {
        public ScheduleFlightCommand(Guid id, DateTime departureTime, DateTime arrivalTime)
        {
            Id = id;
            DepartureTime = departureTime;
            ArrivalTime = arrivalTime;
        }

        public DateTime ArrivalTime { get; }

        public DateTime DepartureTime { get; }

        public Guid Id { get; }
    }
}