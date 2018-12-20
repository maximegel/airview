using System;
using System.Linq;
using System.Reflection;
using AirView.Shared.Railways;

namespace AirView.Domain.Core.Internal
{
    public static class AggregateFactory
    {
        // TODO(maximegelinas): Store found constructors in memory (i.e. static field) to reduce reflexion calls.
        public static TAggregate CreateByReflexion<TAggregate>(object id)
            where TAggregate : IAggregateRoot =>
            typeof(TAggregate)
                .GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .OrderByDescending(ctor => ctor.IsPrivate)
                .ThenBy(ctor => ctor.GetParameters().Length)
                .TryFirst()
                .Map(ctor => (ctor, paramTypes: ctor.GetParameters().Select(param => param.ParameterType)))
                .Map(pair => (
                    pair.ctor,
                    @params: pair.paramTypes
                        .Select(type => type.IsValueType ? Activator.CreateInstance(type) : null)
                        .ToArray()))
                .Map(pair => pair.ctor.Invoke(pair.@params))
                .Map(instance => (TAggregate) instance)
                .Do(aggregate =>
                {
                    aggregate.GetType().GetProperty(nameof(IAggregateRoot.Id))?.SetValue(aggregate, id);
                    aggregate.GetType()
                        .GetProperty(nameof(IAggregateRoot.Version), BindingFlags.NonPublic | BindingFlags.Instance)
                        ?.SetValue(aggregate, 0);
                    aggregate.ClearUncommitedEvents();
                })
                .Reduce(() => throw new InvalidOperationException(
                    $"Constructor not found for aggregate '{typeof(TAggregate).Name}'."));
    }
}