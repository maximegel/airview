using System;
using System.Threading;
using System.Threading.Tasks;
using AirView.Application.Core;
using AirView.Domain;
using AirView.Domain.Core;
using AirView.Persistence.Core;
using AirView.Shared.Railways;

namespace AirView.Application
{
    public class RegisterFlightCommandHandler :
        // TODO(maximegelinas): Create a abstract class that implement 'ICommandHandler<TCommand, Result<CommandException<TCommand>, TSuccess>>'.
        ICommandHandler<RegisterFlightCommand, Result<CommandException<RegisterFlightCommand>, Guid>>
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IWritableRepository<Flight> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterFlightCommandHandler(
            IWritableRepository<Flight> repository, IUnitOfWork unitOfWork, IEventPublisher eventPublisher)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _eventPublisher = eventPublisher;
        }

        public async Task<Result<CommandException<RegisterFlightCommand>, Guid>> HandleAsync(
            RegisterFlightCommand command, CancellationToken cancellationToken)
        {
            var newFlight = new Flight(Guid.NewGuid(), command.Number);

            _repository.Add(newFlight);
            // TODO(maximegelinas): Create a 'TransactionCommandHandlerDecorator' so we don't have to commit the transaction in each command handler.
            await _unitOfWork.CommitAsync(cancellationToken);
            // TODO(maximegelinas): Find a way to avoid having to publish the events in each command handler.
            await _eventPublisher.PublishAsync(newFlight, cancellationToken);

            return newFlight.Id;
        }
    }
}