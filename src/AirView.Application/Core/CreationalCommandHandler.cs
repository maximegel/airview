using System.Threading;
using System.Threading.Tasks;
using AirView.Application.Core.Exceptions;
using AirView.Shared.Railways;

namespace AirView.Application.Core
{
    public abstract class CreationalCommandHandler<TCommand, TEntityId> :
        ICreationalCommandHandler<TCommand, TEntityId>
        where TCommand : ICreationalCommand<Result<CommandException<TCommand>, TEntityId>>
    {
        protected CommandExceptionFactory<TCommand> ExceptionOf =>
            new CommandExceptionFactory<TCommand>();

        public abstract Task<Result<CommandException<TCommand>, TEntityId>> HandleAsync(
            TCommand command, CancellationToken cancellationToken);

        protected Result<CommandException<TCommand>, TEntityId> Ok(TEntityId id) => id;
    }
}