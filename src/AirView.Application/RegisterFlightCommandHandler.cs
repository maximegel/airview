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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWritableRepository<Guid, Flight> _writableRepository;

        public RegisterFlightCommandHandler(
            IWritableRepository<Guid, Flight> writableRepository,
            IUnitOfWork unitOfWork)
        {
            _writableRepository = writableRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CommandException<RegisterFlightCommand>, Guid>> HandleAsync(
            RegisterFlightCommand command, CancellationToken cancellationToken)
        {
            var newFlight = new Flight(Guid.NewGuid(), command.Number);

            _writableRepository.Add(newFlight);
            await _writableRepository.SaveAsync(cancellationToken);
            _unitOfWork.Commit();

            return newFlight.Id;
        }
    }
}