using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using AirView.Shared.Railways;

namespace AirView.Domain.Core.Internal
{
    internal class MethodsEventRouter<TEvent> :
        IEventRouter<TEvent>
    {
        [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
        private static readonly IDictionary<Type, IEnumerable<(Type type, MethodInfo method)>> Routes =
            new ConcurrentDictionary<Type, IEnumerable<(Type type, MethodInfo method)>>();

        private readonly Regex _methodsNamePattern;
        private readonly object _source;

        public MethodsEventRouter(object source, string methodsNamePattern) :
            this(source, new Regex(methodsNamePattern, RegexOptions.IgnoreCase | RegexOptions.Singleline))
        {
        }

        public MethodsEventRouter(object source, Regex methodsNamePattern)
        {
            _source = source;
            _methodsNamePattern = methodsNamePattern;
        }

        public void Dispatch(TEvent @event) =>
            Routes
                .TryGetValue(_source.GetType())
                .Reduce(() => Routes[_source.GetType()] = GetRoutes(_source))
                .Select(route => (route, @event))
                .Where(tuple => tuple.route.type.IsInstanceOfType(tuple.@event))
                .ToList()
                .ForEach(tuple => tuple.route.method.Invoke(_source, new object[] {tuple.@event}));

        private IEnumerable<(Type type, MethodInfo method)> GetRoutes(object source) =>
            source.GetType()
                .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(method =>
                    method.ReturnType == typeof(void) &&
                    method.GetParameters().Length == 1 &&
                    typeof(TEvent).IsAssignableFrom(method.GetParameters().Single().ParameterType) &&
                    _methodsNamePattern.IsMatch(method.Name))
                .Select(method => (
                    method.GetParameters().Single().ParameterType,
                    method));
    }
}