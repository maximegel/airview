using System.Collections.Generic;

namespace AirView.Domain.Core
{
    /// <inheritdoc cref="IEntity{TId}" />
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
    public interface IAggregateRoot<TSelf, TId> :
        IAggregateRoot<TId>
        where TSelf : IAggregateRoot<TSelf, TId>
    {
        new IEnumerable<IDomainEvent<TSelf, TId>> UncommittedEvents { get; }

        void ApplyEvent(IDomainEvent<TSelf, TId> @event);
    }
}