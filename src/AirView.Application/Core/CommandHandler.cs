using System.Threading;
using System.Threading.Tasks;
using AirView.Application.Core.Exceptions;
using AirView.Shared.Railways;

namespace AirView.Application.Core
{
    public abstract class CommandHandler<TCommand> :
        ICommandHandler<TCommand>
        where TCommand : ICommand<Result<CommandException<TCommand>>>
    {
        protected CommandExceptionFactory<TCommand> ExceptionOf =>
            new CommandExceptionFactory<TCommand>();

        protected Result<CommandException<TCommand>> Ok =>
            Result.Success;

        public abstract Task<Result<CommandException<TCommand>>> HandleAsync(
            TCommand command, CancellationToken cancellationToken);
    }
}