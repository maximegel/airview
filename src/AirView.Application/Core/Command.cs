using AirView.Application.Core.Exceptions;
using AirView.Shared.Railways;

namespace AirView.Application.Core
{
    public abstract class Command<TSelf> :
        ICommand<Result<CommandException<TSelf>>>
        where TSelf : Command<TSelf>
    {
    }
}