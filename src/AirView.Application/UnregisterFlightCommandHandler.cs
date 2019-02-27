using System.Threading;
using System.Threading.Tasks;
using AirView.Application.Core;
using AirView.Application.Core.Exceptions;
using AirView.Domain;
using AirView.Domain.Core;
using AirView.Persistence.Core;
using AirView.Shared.Railways;

namespace AirView.Application
{
    public class UnregisterFlightCommandHandler :
        // TODO(maximegelinas): Create a abstract class that implement 'ICommandHandler<TCommand, Result<CommandException<TCommand>>>'.
        ICommandHandler<UnregisterFlightCommand, Result<CommandException<UnregisterFlightCommand>>>
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IWritableRepository<Flight> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UnregisterFlightCommandHandler(
            IWritableRepository<Flight> repository, IUnitOfWork unitOfWork, IEventPublisher eventPublisher)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _eventPublisher = eventPublisher;
        }

        public async Task<Result<CommandException<UnregisterFlightCommand>>> HandleAsync(
            UnregisterFlightCommand command, CancellationToken cancellationToken) =>
            await (await _repository.TryFindAsync(command.Id, cancellationToken))
                .Map(async flight =>
                {
                    _repository.Remove(flight);
                    // TODO(maximegelinas): Create a 'TransactionCommandHandlerDecorator' so we don't have to commit the transaction in each command handler.
                    await _unitOfWork.CommitAsync(cancellationToken);
                    // TODO(maximegelinas): Find a way to avoid having to publish the events in each command handler.
                    await _eventPublisher.PublishAsync(flight, cancellationToken);

                    return (Result<CommandException<UnregisterFlightCommand>>) Result.Success;
                })
                .ReduceAsync(() => new EntityNotFoundCommandException<UnregisterFlightCommand>(command.Id.ToString()));
    }
}