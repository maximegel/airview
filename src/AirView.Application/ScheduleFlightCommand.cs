using System;
using AirView.Application.Core;
using AirView.Shared.Railways;

namespace AirView.Application
{
    public class ScheduleFlightCommand :
        ICommand<Result<CommandException<ScheduleFlightCommand>>>,
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