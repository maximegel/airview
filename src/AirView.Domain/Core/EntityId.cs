namespace AirView.Domain.Core
{
    public abstract class EntityId<TSelf> : ValueObject<TSelf> 
        where TSelf : EntityId<TSelf>
    {
    }
}