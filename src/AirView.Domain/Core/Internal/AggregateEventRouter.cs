using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AirView.Shared.Railways;

namespace AirView.Domain.Core.Internal
{
    internal class AggregateEventRouter
    {
        private static readonly IDictionary<Type, IDictionary<Type, MethodInfo>> Routes =
            new ConcurrentDictionary<Type, IDictionary<Type, MethodInfo>>();

        private readonly IAggregateRoot _aggregate;

        public AggregateEventRouter(IAggregateRoot aggregate) => 
            _aggregate = aggregate;

        public void Dispatch(IAggregateEvent @event) =>
            Routes
                .TryGetValue(_aggregate.GetType())
                .Reduce(() => Routes[_aggregate.GetType()] = GetRoutes(_aggregate))
                .TryGetValue(@event.GetType())
                .Do(method => method.Invoke(_aggregate, new object[] {@event}));


        private static IDictionary<Type, MethodInfo> GetRoutes(IAggregateRoot aggregate) =>
            aggregate.GetType()
                .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(method =>
                    method.Name == "Apply" &&
                    method.GetParameters().Length == 1 &&
                    typeof(IAggregateEvent<,>)
                        .MakeGenericType(aggregate.GetType(), aggregate.Id.GetType())
                        .IsAssignableFrom(method.GetParameters().Single().ParameterType))
                .Select(method => new KeyValuePair<Type, MethodInfo>(
                    method.GetParameters().Single().ParameterType,
                    method))
                .ToDictionary(pair => pair.Key, pair => pair.Value);
    }
}