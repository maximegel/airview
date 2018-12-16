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
    public class ScheduleFlightCommandHandler :
        ICommandHandler<ScheduleFlightCommand, Result<CommandException<ScheduleFlightCommand>>>
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

        public async Task<Result<CommandException<ScheduleFlightCommand>>> HandleAsync(ScheduleFlightCommand command, CancellationToken cancellationToken)
        {
            var commandId = command.Id;
            return await (await _writableRepository.TryFindAsync(commandId, cancellationToken))
                .Map(async flight =>
                {
                    flight.Schedule(command.DepartureTime, command.ArrivalTime);

                    await _writableRepository.SaveAsync(cancellationToken);
                    _unitOfWork.Commit();

                    return (Result<CommandException<ScheduleFlightCommand>>) Result.Success;
                })
                .ReduceAsync(() => new EntityNotFoundCommandException<ScheduleFlightCommand>(commandId.ToString()));
        }
    }
}