using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AirView.Domain.Core;
using AirView.Shared.Railways;

namespace AirView.Application.Core
{
    public class InMemoryBusBuilder
    {
        private IDictionary<Type, ICollection<Delegate>> Handlers { get; } =
            new Dictionary<Type, ICollection<Delegate>>();

        public InMemoryBusBuilder AddCommandHandler<TCommand, TResult>(
            Func<ICommandHandler<TCommand, TResult>> handlerFactory)
            where TCommand : ICommand<TResult> =>
            AddCommandHandler<TCommand, TResult>(handlerFactory().HandleAsync);

        public InMemoryBusBuilder AddCommandHandler<TCommand, TResult>(
            Func<TCommand, CancellationToken, Task<TResult>> handler)
            where TCommand : ICommand<TResult>
        {
            if (Handlers.ContainsKey(typeof(TCommand)))
                throw new InvalidOperationException(
                    $"Cannot register more than one command handler for '{typeof(TCommand).Name}'.");

            Handlers.Add(typeof(TCommand), new List<Delegate> {handler});
            return this;
        }

        public InMemoryBusBuilder AddEventHandler<TEvent>(Func<IEventHandler<TEvent>> handlerFactory)
            where TEvent : IDomainEvent =>
            AddEventHandler<TEvent>(handlerFactory().HandleAsync);

        public InMemoryBusBuilder AddEventHandler<TEvent>(Func<TEvent, CancellationToken, Task> handler)
            where TEvent : IDomainEvent
        {
            Handlers[typeof(TEvent)] = Handlers.TryGetValue(typeof(TEvent))
                .Do(handlers => handlers.Add(handler))
                .Reduce(() => new List<Delegate> {handler});
            return this;
        }

        public InMemoryBus Build() =>
            new InMemoryBus {Handlers = Handlers};
    }
}