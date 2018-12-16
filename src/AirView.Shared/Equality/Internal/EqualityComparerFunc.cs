using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AirView.Shared.Equality.Internal
{
    internal class EqualityComparerFunc<T> : IEqualityComparer<T>
        where T : class
    {
        private readonly Func<T, T, bool> _equalsFunc;
        private readonly Func<T, int> _getHashCodeFunc;

        public EqualityComparerFunc(Func<T, T, bool> equalsFunc, Func<T, int> getHashCodeFunc)
        {
            _equalsFunc = equalsFunc;
            _getHashCodeFunc = getHashCodeFunc;
        }

        public bool Equals(T x, T y) =>
            // True if both references are equal.
            ReferenceEquals(x, y) ||
            // False if only one object is null.
            !(x is null) && !(y is null) &&
            // True if the delegate function validate the equality.
            _equalsFunc(x, y);

        [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
        public int GetHashCode(T obj) =>
            obj is null
                ? 0
                : _getHashCodeFunc(obj);
    }
}