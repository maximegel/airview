using AirView.Shared.Railways.Internal.Either;

namespace AirView.Shared.Railways
{
    public abstract class Either<TLeft, TRight>
    {
        public static Either<TLeft, TRight> From(TLeft obj) => obj;

        public static Either<TLeft, TRight> From(TRight obj) => obj;

        public static implicit operator Either<TLeft, TRight>(TLeft obj) =>
            new Left<TLeft, TRight>(obj);

        public static implicit operator Either<TLeft, TRight>(TRight obj) =>
            new Right<TLeft, TRight>(obj);
    }
}