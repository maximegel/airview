using System;
using System.Collections.Generic;
using System.Linq;
using AirView.Domain.Core;
using AirView.Shared.Railways;

namespace AirView.Application.Core
{
    public class InMemoryBusBuilder
    {
        private IDictionary<Type, ICollection<Delegate>> Routes { get; } =
            new Dictionary<Type, ICollection<Delegate>>();

        public InMemoryBusBuilder AddCommandHandler<TCommand, TResult>(ICommandHandler<TCommand, TResult> handler)
            where TCommand : ICommand<TResult> =>
            AddCommandHandler<TCommand, TResult>(handler.HandleAsync);

        public InMemoryBusBuilder AddCommandHandler<TCommand, TResult>(HandlerDelegate<TCommand, TResult> handler)
            where TCommand : ICommand<TResult>
        {
            if (Routes.ContainsKey(typeof(TCommand)))
                throw new InvalidOperationException(
                    $"Cannot register more than one command handler for the same type: {typeof(TCommand).Name}.");

            Routes.Add(typeof(TCommand), new List<Delegate> {handler});
            return this;
        }

        public InMemoryBusBuilder AddCommandHandlers<TCommand, TResult>(
            IEnumerable<ICommandHandler<TCommand, TResult>> handlers)
            where TCommand : ICommand<TResult> =>
            handlers.Aggregate(this, (current, next) => current.AddCommandHandler(next));

        public InMemoryBusBuilder AddEventHandler<TEvent>(HandlerDelegate<TEvent> handler)
            where TEvent : IDomainEvent
        {
            Routes[typeof(TEvent)] =
                Routes.TryGetValue(typeof(TEvent))
                    .Map(handlers =>
                    {
                        handlers.Add(handler);
                        return handlers;
                    })
                    .Reduce(() => new List<Delegate> {handler});

            return this;
        }

        public InMemoryBus Build() =>
            new InMemoryBus
            {
                Routes = Routes
            };
    }
}