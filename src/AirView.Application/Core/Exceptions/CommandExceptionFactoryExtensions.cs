namespace AirView.Application.Core.Exceptions
{
    public static class CommandExceptionFactoryExtensions
    {
        public static EntityNotFoundCommandException<TCommand> EntityNotFound<TCommand>(
            this CommandExceptionFactory<TCommand> factory, string id) 
            where TCommand : IAccessOptionalEntityCommand =>
            new EntityNotFoundCommandException<TCommand>(id);
    }
}