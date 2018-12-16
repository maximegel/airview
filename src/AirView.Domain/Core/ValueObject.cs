using System.Collections.Generic;
using AirView.Shared.Equality;

namespace AirView.Domain.Core
{
    /// <inheritdoc />
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
    /// <typeparam name="TSelf"></typeparam>
    // TODO(maximegelinas): Simplify code by following this implementation: https://enterprisecraftsmanship.com/2017/08/28/value-object-a-better-implementation/. 
    public abstract class ValueObject<TSelf> : Equatable<TSelf>
        where TSelf : ValueObject<TSelf>
    {
        public abstract override string ToString();

        protected override IEqualityComparer<TSelf> GetEqualityComparer() =>
            EqualityComparer.ByTaggedMembers<TSelf>();
    }
}