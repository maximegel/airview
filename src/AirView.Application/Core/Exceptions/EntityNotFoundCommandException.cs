namespace AirView.Application.Core.Exceptions
{
    public class EntityNotFoundCommandException<TCommand> : CommandException<TCommand> 
        where TCommand : IAccessOptionalEntityCommand
    {
        public EntityNotFoundCommandException(string id) => 
            Id = id;

        public string Id { get; }
    }
}