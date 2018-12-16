using System;
using System.Threading;
using System.Threading.Tasks;
using AirView.Application.Core;
using AirView.Application.Core.Exceptions;
using AirView.Domain;
using AirView.Persistence.Core;
using AirView.Shared.Railways;

namespace AirView.Application
{
    public class RegisterFlightCommandHandler : CreationalCommandHandler<RegisterFlightCommand, Guid>
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

        public override async Task<Result<CommandException<RegisterFlightCommand>, Guid>> HandleAsync(
            RegisterFlightCommand command, CancellationToken cancellationToken)
        {
            var newFlight = new Flight(Guid.NewGuid(), command.Number);

            _writableRepository.Add(newFlight);
            await _writableRepository.SaveAsync(cancellationToken);
            _unitOfWork.Commit();

            return Ok(newFlight.Id);
        }
    }
}