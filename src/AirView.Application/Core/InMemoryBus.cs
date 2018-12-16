using System;
using System.Collections.Generic;
using System.Linq;
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
            Handlers.TryGetValue(@event.GetType())
                .Map(handlers => handlers.Select(handler => (Task) handler.DynamicInvoke(@event, cancellationToken)))
                .Map(handlers => handlers.Select(async task => await task.ConfigureAwait(false)))
                .Map(Task.WhenAll)
                .Reduce(Task.CompletedTask);
    }
}