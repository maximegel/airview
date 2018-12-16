using System;
using AirView.Application.Core;
using AirView.Shared.Railways;

namespace AirView.Application
{
    public class UnregisterFlightCommand :
        ICommand<Result<CommandException<UnregisterFlightCommand>>>,
        IAccessOptionalEntityCommand
    {
        public UnregisterFlightCommand(Guid id) =>
            Id = id;

        public Guid Id { get; }
    }
}