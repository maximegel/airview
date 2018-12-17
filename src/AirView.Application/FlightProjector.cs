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
        IEventHandler<IDomainEvent<Flight, Guid, FlightRegistratedEvent>>,
        IEventHandler<IDomainEvent<Flight, Guid, FlightScheduledEvent>>
    {
        private readonly IWritableRepository<Guid, FlightProjection> _repository;
        private readonly IReadUnitOfWork _unitOfWork;

        public FlightProjector(IWritableRepository<Guid, FlightProjection> repository, IReadUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(
            IDomainEvent<Flight, Guid, FlightRegistratedEvent> @event, CancellationToken cancellationToken)
        {
            _repository.Add(new FlightProjection(@event.AggregateId)
            {
                Number = @event.Data.Number
            });

            await _repository.SaveAsync(cancellationToken);
            _unitOfWork.Commit();
        }

        public async Task HandleAsync(
            IDomainEvent<Flight, Guid, FlightScheduledEvent> @event, CancellationToken cancellationToken)
        {
            var id = @event.AggregateId;
            (await _repository.TryFindAsync(id, cancellationToken))
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