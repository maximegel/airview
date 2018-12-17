using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AirView.Domain.Core;
using AirView.Shared.Railways;

namespace AirView.Application.Core
{
    public class InMemoryBus :
        ICommandSender,
        IEventPublisher
    {
        internal InMemoryBus()
        {
        }

        internal IDictionary<Type, ICollection<Delegate>> Handlers { get; set; }

        public Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken) =>
            Handlers.TryGetValue(command.GetType())
                .Map(handlers => handlers.Select(handler => handler))
                .Map(handlers => handlers.Single())
                .Map(handler => (Task<TResult>) handler.DynamicInvoke(command, cancellationToken))
                .Reduce(() => throw new InvalidOperationException(
                    $"No command handler registrated for '{command.GetType().Name}'."));

        public Task PublishAsync(IDomainEvent @event, CancellationToken cancellationToken) =>
            Task.WhenAll(Handlers
                .AsEnumerable()
                .Where(pair => pair.Key.IsInstanceOfType(@event))
                .SelectMany(pair => pair.Value)
                .Select(handler => (Task) handler.DynamicInvoke(@event, cancellationToken))
                .Select(async task => await task.ConfigureAwait(false)));
    }
}