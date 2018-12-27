using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace AirView.Domain.Core
{
    /// <inheritdoc cref="IEntity{TId}" />
    /// <summary>
    ///     Represents an object that is not fundamentally defined by its properties, but rather by its thread of continuity
    ///     and its identity.
    ///     <see href="https://lostechies.com/jimmybogard/2008/05/21/entities-value-objects-aggregates-and-roots/" />
    /// </summary>
    /// <example>
    ///     A person is a good example of entity because a person can't be defined by its properties (name, sex, address etc.).
    ///     <code>
    ///          public class Person : Entity;
    ///          {
    ///              // Constructs a existing person.
    ///              public Person(Guid id, string name) :
    ///                  base(id) =&gt;
    ///                  Name = name;
    /// 
    ///              // Constructs a new person.
    ///              public Person(string name) :
    ///                  this(Guid.NewGuid(), name)
    ///              {
    ///              }
    /// 
    ///              public string Name { get; }
    ///          }
    ///      </code>
    /// </example>
    /// <typeparam name="TId"></typeparam>
    public abstract class Entity<TId> :
        IEntity<TId>
    {
        protected Entity(TId id) =>
            Id = id;

        object IEntity.Id => Id;

        [JsonProperty(Order = -9)]
        public TId Id { get; protected set; }

        public static bool operator ==(Entity<TId> left, IEntity right) =>
            Equals(left, right);

        public static bool operator !=(Entity<TId> left, IEntity right) =>
            !(left == right);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return GetType() == obj.GetType() && Equals((Entity<TId>) obj);
        }

        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode() =>
            EqualityComparer<TId>.Default.GetHashCode(Id);

        public override string ToString() =>
            $"{GetType()}#{Id}";

        protected bool Equals(Entity<TId> other) =>
            EqualityComparer<TId>.Default.Equals(Id, other.Id);
    }
}