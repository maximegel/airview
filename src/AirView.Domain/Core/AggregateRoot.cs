using System;
using System.Collections.Generic;
using AirView.Domain.Core.Internal;
using AirView.Shared;

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
    /// <typeparam name="TId"></typeparam>
    public abstract class AggregateRoot<TId> : Entity<TId>,
        IAggregateRoot<TId>
    {
        private readonly AggregateEventRouter _router;
        private readonly ICollection<IDomainEvent> _uncommitedEvents = new List<IDomainEvent>();

        protected AggregateRoot(TId id) :
            base(id)
        {
            _router = new AggregateEventRouter(this);
            Name = GetType().Name;
        }

        protected virtual string Name { get; }

        protected long Version { get; set; }

        void IAggregateRoot.ApplyEvent(IDomainEvent @event)
        {
            if (@event.AggregateVersion != Version + 1)
                throw new ArgumentException(
                    $"Domain event '{@event.GetType().GetFriendlyName()}' with aggregate version of {@event.AggregateVersion} " +
                    $"cannot be applied to aggregate '{GetType().GetFriendlyName()}' at version {Version}.");
            if (!typeof(IDomainEvent<>).MakeGenericType(GetType()).IsInstanceOfType(@event))
                throw new ArgumentException(
                    $"Domain event '{@event.GetType().GetFriendlyName()}' cannot be applied to aggregate '{GetType().GetFriendlyName()}'.");

            _router.Dispatch(@event.Data);
            Version = @event.AggregateVersion;
        }

        void IAggregateRoot.ClearUncommitedEvents() =>
            _uncommitedEvents.Clear();

        object IAggregateRoot.Id => Id;

        void IAggregateRoot.RaiseEvent<TEvent>(TEvent @event) =>
            Raise(@event);

        IEnumerable<IDomainEvent> IAggregateRoot.UncommittedEvents =>
            _uncommitedEvents;

        long IAggregateRoot.Version => Version;

        protected void Raise<TEvent>(TEvent @event)
            where TEvent : IAggregateEvent
        {
            var domainEvent = DomainEvent.Of(GetType(), Id, ++Version, @event);
            _router.Dispatch(domainEvent.Data);
            _uncommitedEvents.Add(domainEvent);
        }
    }
}