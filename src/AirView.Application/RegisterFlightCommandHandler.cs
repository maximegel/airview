using System;
using System.Threading;
using System.Threading.Tasks;
using AirView.Application.Core;
using AirView.Domain;
using AirView.Persistence.Core;
using AirView.Shared.Railways;

namespace AirView.Application
{
    public class RegisterFlightCommandHandler :
        // TODO(maximegelinas): Create a abstract class that implement 'ICommandHandler<TCommand, Result<CommandException<TCommand>, TSuccess>>'.
        ICommandHandler<RegisterFlightCommand, Result<CommandException<RegisterFlightCommand>, Guid>>
    {
        private readonly IWritableRepository<Flight> _repository;        
        private readonly IUnitOfWork _unitOfWork;

        public RegisterFlightCommandHandler(IWritableRepository<Flight> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CommandException<RegisterFlightCommand>, Guid>> HandleAsync(
            RegisterFlightCommand command, CancellationToken cancellationToken)
        {
            var newFlight = new Flight(Guid.NewGuid(), command.Number);

            _repository.Add(newFlight);
            // TODO(maximegelinas): Create a 'TransactionCommandHandlerDecorator' so we don't have to commit the transaction in each command handler.
            await _unitOfWork.CommitAsync(cancellationToken);

            return newFlight.Id;
        }
    }
}