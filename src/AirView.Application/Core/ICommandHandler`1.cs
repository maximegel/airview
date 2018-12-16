using AirView.Application.Core.Exceptions;
using AirView.Shared.Railways;

namespace AirView.Application.Core
{
    public interface ICommandHandler<TCommand> :
        ICommandHandler<TCommand, Result<CommandException<TCommand>>>
        where TCommand : ICommand<Result<CommandException<TCommand>>>
    {
    }
}