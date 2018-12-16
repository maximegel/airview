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
    public class ScheduleFlightCommandHandler : CommandHandler<ScheduleFlightCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWritableRepository<Guid, Flight> _writableRepository;

        public ScheduleFlightCommandHandler(
            IWritableRepository<Guid, Flight> writableRepository,
            IUnitOfWork unitOfWork)
        {
            _writableRepository = writableRepository;
            _unitOfWork = unitOfWork;
        }

        public override async Task<Result<CommandException<ScheduleFlightCommand>>> HandleAsync(
            ScheduleFlightCommand command, CancellationToken cancellationToken) =>
            await (await _writableRepository.TryFindAsync(command.Id, cancellationToken))
                .Map(async flight =>
                {
                    flight.Schedule(command.DepartureTime, command.ArrivalTime);

                    await _writableRepository.SaveAsync(cancellationToken);
                    _unitOfWork.Commit();

                    return Ok;
                })
                .ReduceAsync(ExceptionOf.EntityNotFound(command.Id.ToString()));
    }
}