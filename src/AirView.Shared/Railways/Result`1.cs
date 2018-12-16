using AirView.Shared.Railways.Internal.Result;

namespace AirView.Shared.Railways
{
    public abstract class Result<TFailure>
    {
        public static implicit operator Result<TFailure>(TFailure failure) =>
            new Failure<TFailure>(failure);

        public static implicit operator Result<TFailure>(Success success) =>
            new Success<TFailure>();
    }
}