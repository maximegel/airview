using System;

namespace AirView.Application.Core
{
    public class CommandException<TCommand> : ApplicationException
        where TCommand : ICommand
    {
    }
}