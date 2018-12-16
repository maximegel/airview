using System;
using AirView.Shared.Railways;

namespace AirView.Application.Core.Exceptions
{
    public static class ResultExtensions
    {
        public static Result<CommandException<TCommand>, TSuccess> ReduceEntityNotFound<TCommand, TSuccess>(
            this Result<CommandException<TCommand>, TSuccess> result, Func<CommandException<TCommand>, TSuccess> mapper)
            where TCommand : IAccessOptionalEntityCommand =>
            result.Reduce<EntityNotFoundCommandException<TCommand>>(mapper);
    }
}