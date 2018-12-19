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
    public class FlightProjector :
        IEventHandler<IDomainEvent<Flight, FlightRegistratedEvent>>,
        IEventHandler<IDomainEvent<Flight, FlightScheduledEvent>>,
        IEventHandler<IDomainEvent<Flight, AggregateRemovedEvent>>
    {
        private readonly IWritableRepository<Guid, FlightProjection> _repository;
        private readonly IReadUnitOfWork _unitOfWork;

        public FlightProjector(IWritableRepository<Guid, FlightProjection> repository, IReadUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(
            IDomainEvent<Flight, AggregateRemovedEvent> @event, CancellationToken cancellationToken)
        {
            var id = @event.AggregateId;
            (await _repository.TryFindAsync((Guid) id, cancellationToken))
                .Do(async flight =>
                {
                    _repository.Remove(flight);
                    await _repository.SaveAsync(cancellationToken);
                    _unitOfWork.Commit();
                })
                .Reduce(() => throw new ApplicationException($"Flight projection with identifier '{id}' not found."));
        }

        public async Task HandleAsync(
            IDomainEvent<Flight, FlightRegistratedEvent> @event, CancellationToken cancellationToken)
        {
            _repository.Add(new FlightProjection((Guid) @event.AggregateId)
            {
                Number = @event.Data.Number
            });

            await _repository.SaveAsync(cancellationToken);
            _unitOfWork.Commit();
        }

        public async Task HandleAsync(
            IDomainEvent<Flight, FlightScheduledEvent> @event, CancellationToken cancellationToken)
        {
            var id = @event.AggregateId;
            (await _repository.TryFindAsync((Guid) id, cancellationToken))
                .Do(async flight =>
                {
                    flight.DepartureTime = @event.Data.DepartureTime;
                    flight.ArrivalTime = @event.Data.DepartureTime;

                    await _repository.SaveAsync(cancellationToken);
                    _unitOfWork.Commit();
                })
                .Reduce(() => throw new ApplicationException($"Flight projection with identifier '{id}' not found."));
        }
    }
}