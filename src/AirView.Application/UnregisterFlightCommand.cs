using System;
using AirView.Application.Core;

namespace AirView.Application
{
    public class UnregisterFlightCommand : Command<UnregisterFlightCommand>,
        IAccessOptionalEntityCommand
    {
        public UnregisterFlightCommand(Guid id) =>
            Id = id;

        public Guid Id { get; }
    }
}