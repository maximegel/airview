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
        ICommandHandler<RegisterFlightCommand, Result<CommandException<RegisterFlightCommand>, Guid>>
    {
        private readonly IWritableRepository<Guid, Flight> _repository;
        private readonly IWriteUnitOfWork _unitOfWork;

        public RegisterFlightCommandHandler(IWritableRepository<Guid, Flight> repository, IWriteUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CommandException<RegisterFlightCommand>, Guid>> HandleAsync(
            RegisterFlightCommand command, CancellationToken cancellationToken)
        {
            var newFlight = new Flight(Guid.NewGuid(), command.Number);

            _repository.Add(newFlight);
            await _repository.SaveAsync(cancellationToken);
            _unitOfWork.Commit();

            return newFlight.Id;
        }
    }
}