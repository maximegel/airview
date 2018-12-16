using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AirView.Domain.Core;
using AirView.Shared.Railways;

namespace AirView.Application.Core
{
    // TODO(maximegelinas): Remove builder and use a "RegisterHandler" method instead.
    public class InMemoryBus :
        ICommandSender,
        IEventPublisher
    {
        internal InMemoryBus()
        {
        }

        internal IDictionary<Type, ICollection<Delegate>> Routes { get; set; }

        public Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken) =>
            Routes.TryGetValue(command.GetType())
                .Map(handlers => handlers.Select(handler => handler))
                .Map(handlers => handlers.Single())
                .Map(handler => (Task<TResult>) handler.DynamicInvoke(command, cancellationToken))
                .Reduce(() => throw new InvalidOperationException(
                    $"No command handler registrated for type: {command.GetType().Name}."));

        public Task PublishAsync(IDomainEvent @event, CancellationToken cancellationToken) =>
            Routes.TryGetValue(@event.GetType())
                .Map(handlers => handlers.Select(handler => (HandlerDelegate<IDomainEvent>) handler).ToList())
                .Map(handlers => handlers.Select(handler => handler(@event, cancellationToken)))
                .Map(handlers => handlers.Select(async task => await task.ConfigureAwait(false)))
                .Map(Task.WhenAll)
                .Reduce(Task.CompletedTask);
    }
}