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
    public class ScheduleFlightCommandHandler :
        // TODO(maximegelinas): Create a abstract class that implement 'ICommandHandler<TCommand, Result<CommandException<TCommand>>>'.
        ICommandHandler<ScheduleFlightCommand, Result<CommandException<ScheduleFlightCommand>>>
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IWritableRepository<Flight> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ScheduleFlightCommandHandler(
            IWritableRepository<Flight> repository, IUnitOfWork unitOfWork, IEventPublisher eventPublisher)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _eventPublisher = eventPublisher;
        }

        public async Task<Result<CommandException<ScheduleFlightCommand>>> HandleAsync(
            ScheduleFlightCommand command, CancellationToken cancellationToken)
        {
            var commandId = command.Id;
            return await (await _repository.TryFindAsync(commandId, cancellationToken))
                .Map(async flight =>
                {
                    flight.Schedule(command.DepartureTime, command.ArrivalTime);

                    // TODO(maximegelinas): Create a 'TransactionCommandHandlerDecorator' so we don't have to commit the transaction in each command handler.
                    await _unitOfWork.CommitAsync(cancellationToken);
                    // TODO(maximegelinas): Find a way to avoid having to publish the events in each command handler.
                    await _eventPublisher.PublishAsync(flight, cancellationToken);

                    return (Result<CommandException<ScheduleFlightCommand>>) Result.Success;
                })
                .ReduceAsync(() => new EntityNotFoundCommandException<ScheduleFlightCommand>(commandId.ToString()));
        }
    }
}