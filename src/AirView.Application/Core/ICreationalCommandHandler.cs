using AirView.Application.Core.Exceptions;
using AirView.Shared.Railways;

namespace AirView.Application.Core
{
    public interface ICreationalCommandHandler<TCommand, TEntityId> :
        ICommandHandler<TCommand, Result<CommandException<TCommand>, TEntityId>>
        where TCommand : ICreationalCommand<Result<CommandException<TCommand>, TEntityId>>
    {
    }
}