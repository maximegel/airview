using System.Collections.Generic;

namespace AirView.Domain.Core
{
    /// <inheritdoc cref="IEntity" />
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
    public interface IAggregateRoot
    {
        /// <summary>
        ///     Identifier that make the aggreggate root unique.
        /// </summary>
        object Id { get; }

        string Name { get; }

        IEnumerable<IDomainEvent> UncommittedEvents { get; }

        long Version { get; }

        void ApplyEvent(IDomainEvent @event);

        void ClearUncommitedEvents();

        void RaiseEvent<TEvent>(TEvent @event)
            where TEvent : IAggregateEvent;
    }
}