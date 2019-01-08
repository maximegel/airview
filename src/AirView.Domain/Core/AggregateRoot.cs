using System;
using System.Linq;
using System.Reflection;
using AirView.Shared;
using AirView.Shared.Railways;

namespace AirView.Domain.Core
{
    public static class AggregateRoot
    {
        // TODO(maximegelinas): Store found constructors in memory (i.e. static field) to reduce reflexion calls.
        public static TAggregate New<TAggregate>(object id)
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
                .Map(pair =>
                {
                    try
                    {
                        return pair.ctor.Invoke(pair.@params);
                    }
                    catch (Exception e)
                    {
                        throw new AggregateConstructionException(
                            $"Failed to construct aggregate '{typeof(TAggregate).GetFriendlyName()}' using constructor '{pair.ctor.GetFriendlySignature()}'. " +
                            "This is probably du to arguments validation in this constructor. To fix this issue, try to add a private parameterless constructor " +
                            $"without any logic in the class you want to construct (i.e. {typeof(TAggregate).GetFriendlyName()}).",
                            e);
                    }
                })
                .Map(instance => (TAggregate) instance)
                .Do(aggregate =>
                {
                    aggregate.GetType().GetProperty(nameof(IAggregateRoot.Id))?.SetValue(aggregate, id);
                    aggregate.GetType()
                        .GetProperty(nameof(IAggregateRoot.Version), BindingFlags.NonPublic | BindingFlags.Instance)
                        ?.SetValue(aggregate, 0);
                    aggregate.ClearUncommitedEvents();
                })
                .Reduce(() => throw new AggregateConstructionException(
                    $"Constructor not found for aggregate '{typeof(TAggregate).GetFriendlyName()}'."));
    }
}