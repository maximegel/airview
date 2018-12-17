using System;
using System.Collections.Generic;
using AirView.Domain.Core.Internal;

namespace AirView.Domain.Core
{
    /// <inheritdoc cref="Entity{TId}" />
    /// <summary>
    ///     Represents the entry point or the root of the aggregate.
    ///     An aggregate is a cluster of associated objects that we treat as a unit for the purpose of data changes. Each
    ///     aggregate has a root and a boundary. The boundary defines what is inside the aggregate. The root is a single,
    ///     specific entity contained in the aggregate.
    ///     <see href="https://lostechies.com/jimmybogard/2008/05/21/entities-value-objects-aggregates-and-roots/" />
    /// </summary>
    /// <remarks>
    ///     The root is the only member of the aggregate that outside objects are allowed to hold or references to.
    /// </remarks>
    /// <typeparam name="TSelf"></typeparam>
    /// <typeparam name="TId"></typeparam>
    public abstract class AggregateRoot<TSelf, TId> : Entity<TId>,
        IAggregateRoot<TSelf, TId>
        where TSelf : AggregateRoot<TSelf, TId>
    {
        private readonly AggregateEventRouter _router;
        private readonly ICollection<IDomainEvent<TSelf, TId>> _uncommitedEvents = new List<IDomainEvent<TSelf, TId>>();

        protected AggregateRoot(TId id) :
            base(id) =>
            _router = new AggregateEventRouter(this);

        void IAggregateRoot.ApplyEvent(IDomainEvent @event)
        {
            if (!(@event is IDomainEvent<TSelf, TId>))
                throw new ArgumentException(
                    $"Domain event '{@event.GetType().Name}' cannot be applied to aggregate '{GetType().Name}'.");

            ApplyEvent((IDomainEvent<TSelf, TId>) @event);
        }

        void IAggregateRoot.ClearUncommitedEvents() =>
            _uncommitedEvents.Clear();

        object IAggregateRoot.Id => Id;

        IEnumerable<IDomainEvent> IAggregateRoot.UncommittedEvents =>
            _uncommitedEvents;

        public long Version { get; protected set; }

        void IAggregateRoot<TSelf, TId>.ApplyEvent(IDomainEvent<TSelf, TId> @event) =>
            ApplyEvent(@event);

        IEnumerable<IDomainEvent<TSelf, TId>> IAggregateRoot<TSelf, TId>.UncommittedEvents =>
            _uncommitedEvents;

        protected void Raise<TEvent>(TEvent @event)
            where TEvent : IAggregateEvent<TSelf, TId>
        {
            var domainEvent = new DomainEvent<TSelf, TId, TEvent>(Id, ++Version, @event);
            _router.Dispatch(domainEvent.Data);
            _uncommitedEvents.Add(domainEvent);
        }

        private void ApplyEvent(IDomainEvent<TSelf, TId> @event)
        {
            if (@event.AggregateVersion != Version + 1)
                throw new ArgumentException(
                    $"Domain event '{@event.GetType().Name}' with aggregate version of {@event.AggregateVersion} " +
                    $"cannot be applied to aggregate '{GetType().Name}' at version {Version}.");

            _router.Dispatch(@event.Data);
            Version = @event.AggregateVersion;
        }
    }
}