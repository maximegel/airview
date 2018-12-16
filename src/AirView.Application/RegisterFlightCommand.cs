using System;
using AirView.Application.Core;

namespace AirView.Application
{
    public class RegisterFlightCommand : CreationalCommand<RegisterFlightCommand, Guid>
    {
        public RegisterFlightCommand(string number) => 
            Number = number;

        public string Number { get; }
    }
}