using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AirView.Domain.Core
{
    /// <summary>
    ///     Represents an objects without conceptual identity. Value objects also describe a characteristic of a thing and are
    ///     distinguishable only by the state of their properties.
    ///     <see href="https://lostechies.com/jimmybogard/2008/05/21/entities-value-objects-aggregates-and-roots/" />
    /// </summary>
    /// <remarks>
    ///     The value objects never changes their properties from the moment it is created until the moment it is destroyed.
    ///     Following this rule, each operation of the value object class must return a new instance of that value object
    ///     instead of modifying it.
    /// </remarks>
    /// <example>
    ///     An amount of money is a good example of value object because at the opposite of a person an amount of money can be
    ///     defined by its properties.
    ///     <code>
    ///          public sealed class MoneyAmount : ValueObject&lt;MoneyAmount&gt;
    ///          {
    ///              public MoneyAmount(decimal amount, string currencySymbol)
    ///              {
    ///                  Amount = amount;
    ///                  CurrencySymbol = currencySymbol;
    ///              }
    /// 
    ///              public decimal Amount { get; }
    /// 
    ///              public string CurrencySymbol { get; }
    /// 
    ///              // We create a new instance instead of changing properties to conserve immutability.
    ///              public static MoneyAmount operator *(MoneyAmount moneyAmount, decimal factor) =&gt;
    ///                  new MoneyAmount(moneyAmount.Amount * factor, moneyAmount.CurrencySymbol);
    ///          }
    ///      </code>
    /// </example>
    public abstract class ValueObject
    {
        private static readonly ConcurrentDictionary<Type, IReadOnlyCollection<PropertyInfo>> TypeProperties =
            new ConcurrentDictionary<Type, IReadOnlyCollection<PropertyInfo>>();

        public static bool operator ==(ValueObject left, ValueObject right) =>
            Equals(left, right);

        public static bool operator !=(ValueObject left, ValueObject right) =>
            !(left == right);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return GetType() == obj.GetType() && Equals((ValueObject) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return GetEqualityComponents()
                    .Aggregate(17, (current, obj) => current * 23 + (obj?.GetHashCode() ?? 0));
            }
        }

        public abstract override string ToString();

        protected bool Equals(ValueObject other) =>
            GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());

        protected virtual IEnumerable<object> GetEqualityComponents() =>
            GetProperties().Select(property => property.GetValue(this));

        protected virtual IEnumerable<PropertyInfo> GetProperties() =>
            TypeProperties.GetOrAdd(GetType(), type =>
                type.GetTypeInfo()
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .OrderBy(property => property.Name)
                    .ToList());
    }
}