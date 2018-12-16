using System;

namespace AirView.Domain.Core
{
    public interface IDomainEvent
    {
        object AggregateId { get; }

        Type AggregateType { get; }

        long AggregateVersion { get; }

        IAggregateEvent Data { get; }
    }
}