using System;
using AirView.Shared.Railways.Internal.Result;

namespace AirView.Shared.Railways
{
    public abstract class Result<TFailure, TSuccess>
    {
        public static implicit operator Result<TFailure, TSuccess>(TFailure failure) =>
            new Failure<TFailure, TSuccess>(failure);

        public static implicit operator Result<TFailure, TSuccess>(TSuccess success) =>
            new Success<TFailure, TSuccess>(success);

        /// <summary>
        ///     Maps the failure to a success if the failure is assignable to the given type <typeparamref name="TMatch" />.
        /// </summary>
        /// <remarks>
        ///     This method is part of this class instead of being an extension method for the sake of syntactic sugar only. We do
        ///     not want to provide <typeparamref name="TFailure" /> and <typeparamref name="TSuccess" /> when calling this method.
        /// </remarks>
        /// <typeparam name="TMatch">The type to match against the failure.</typeparam>
        /// <param name="map">The mapping function to execute.</param>
        /// <returns>The mapped success if the failure has been match or the original failure if not.</returns>
        public Result<TFailure, TSuccess> Reduce<TMatch>(Func<TMatch, TSuccess> map)
            where TMatch : TFailure =>
            this is Failure<TFailure, TSuccess> failure && (TFailure) failure is TMatch match
                ? map(match)
                : this;
    }
}