using System;
using System.Threading;
using System.Threading.Tasks;
using AirView.Application.Core;
using AirView.Application.Core.Exceptions;
using AirView.Domain;
using AirView.Persistence.Core;
using AirView.Shared.Railways;

namespace AirView.Application
{
    public class UnregisterFlightCommandHandler :
        ICommandHandler<UnregisterFlightCommand, Result<CommandException<UnregisterFlightCommand>>>
    {
        private readonly IWritableRepository<Flight> _repository;
        private readonly IWriteUnitOfWork _unitOfWork;

        public UnregisterFlightCommandHandler(IWritableRepository<Flight> repository, IWriteUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CommandException<UnregisterFlightCommand>>> HandleAsync(
            UnregisterFlightCommand command, CancellationToken cancellationToken)
        {
            var commandId = command.Id;
            return await (await _repository.TryFindAsync(command.Id, cancellationToken))
                .Map(async flight =>
                {
                    _repository.Remove(flight);
                    await _repository.SaveAsync(cancellationToken);
                    _unitOfWork.Commit();

                    return (Result<CommandException<UnregisterFlightCommand>>) Result.Success;
                })
                .ReduceAsync(() => new EntityNotFoundCommandException<UnregisterFlightCommand>(commandId.ToString()));
        }
    }
}