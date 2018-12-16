using System.Collections.Generic;
using AirView.Shared.Equality;

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
    public abstract class Entity<TId> : Equatable<Entity<TId>>,
        IEntity<TId>
    {
        protected Entity(TId id) =>
            Id = id;

        object IEntity.Id => Id;

        public TId Id { get; protected set; }

        public bool Equals(IEntity<TId> other) =>
            base.Equals(other as Entity<TId>);

        public override string ToString() =>
            $"{GetType()}#{Id}";

        protected override IEqualityComparer<Entity<TId>> GetEqualityComparer() =>
            EqualityComparer.ByKey<Entity<TId>>(self => self.Id);
    }
}