using System;

namespace AirView.Application.Core.Exceptions
{
    public abstract class CommandException<TCommand> : ApplicationException
        where TCommand : ICommand
    {
    }
}