using AirView.Application.Core.Exceptions;
using AirView.Shared.Railways;

namespace AirView.Application.Core
{
    public abstract class CreationalCommand<TSelf, TEntityId> :
        ICreationalCommand<Result<CommandException<TSelf>, TEntityId>>
        where TSelf : CreationalCommand<TSelf, TEntityId>
    {
    }
}