using System;
using AirView.Application.Core;
using AirView.Shared.Railways;

namespace AirView.Application
{
    public class RegisterFlightCommand :
        ICommand<Result<CommandException<RegisterFlightCommand>, Guid>>
    {
        public RegisterFlightCommand(string number) =>
            Number = number;

        public string Number { get; }
    }
}