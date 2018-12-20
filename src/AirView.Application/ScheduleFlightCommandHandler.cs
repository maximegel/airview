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
        // TODO(maximegelinas): Create a abstract class that implement 'ICommandHandler<TCommand, Result<CommandException<TCommand>>>'.
        ICommandHandler<ScheduleFlightCommand, Result<CommandException<ScheduleFlightCommand>>>
    {
        private readonly IWritableRepository<Flight> _repository;
        private readonly IWriteUnitOfWork _unitOfWork;

        public ScheduleFlightCommandHandler(IWritableRepository<Flight> repository, IWriteUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CommandException<ScheduleFlightCommand>>> HandleAsync(
            ScheduleFlightCommand command, CancellationToken cancellationToken)
        {
            var commandId = command.Id;
            return await (await _repository.TryFindAsync(commandId, cancellationToken))
                .Map(async flight =>
                {
                    flight.Schedule(command.DepartureTime, command.ArrivalTime);

                    await _repository.SaveAsync(cancellationToken);
                    _unitOfWork.Commit();

                    return (Result<CommandException<ScheduleFlightCommand>>) Result.Success;
                })
                .ReduceAsync(() => new EntityNotFoundCommandException<ScheduleFlightCommand>(commandId.ToString()));
        }
    }
}