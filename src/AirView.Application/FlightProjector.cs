using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AirView.Application.Core;
using AirView.Domain;
using AirView.Domain.Core;
using AirView.Persistence.Core;
using AirView.Persistence.Core.EventSourcing;
using AirView.Shared.Railways;

namespace AirView.Application
{
    public class FlightProjector :
        IEventHandler<IDomainEvent<Flight, FlightRegistratedEvent>>,
        IEventHandler<IDomainEvent<Flight, FlightScheduledEvent>>,
        IEventHandler<IDomainEvent<Flight, AggregateRemovedEvent>>
    {
        private readonly IWritableRepository<FlightProjection> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public FlightProjector(IWritableRepository<FlightProjection> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(
            IDomainEvent<Flight, AggregateRemovedEvent> @event, CancellationToken cancellationToken)
        {
            var id = @event.AggregateId;
            (await _repository.TryFindAsync(id, cancellationToken))
                .Do(async projection =>
                {
                    _repository.Remove(projection);
                    await _unitOfWork.CommitAsync(cancellationToken);
                })
                .Reduce(() => throw new ApplicationException($"Flight projection with identifier '{id}' not found."));
        }

        public async Task HandleAsync(
            IDomainEvent<Flight, FlightRegistratedEvent> @event, CancellationToken cancellationToken)
        {
            var projection = new FlightProjection((Guid) @event.AggregateId);
            projection.ApplyEvent(@event);

            _repository.Add(projection);
            await _unitOfWork.CommitAsync(cancellationToken);
        }

        public async Task HandleAsync(
            IDomainEvent<Flight, FlightScheduledEvent> @event, CancellationToken cancellationToken)
        {
            var id = @event.AggregateId;
            (await _repository.TryFindAsync(id, cancellationToken))
                .Do(async projection =>
                {
                    projection.ApplyEvent(@event);

                    await _unitOfWork.CommitAsync(cancellationToken);
                })
                .Reduce(() =>
                    throw new ApplicationException($"Flight projection with identifier '{id}' not found."));
        }
    }
}