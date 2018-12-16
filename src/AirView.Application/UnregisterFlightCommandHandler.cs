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
    public class UnregisterFlightCommandHandler : CommandHandler<UnregisterFlightCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWritableRepository<Guid, Flight> _writableRepository;

        public UnregisterFlightCommandHandler(
            IWritableRepository<Guid, Flight> writableRepository,
            IUnitOfWork unitOfWork)
        {
            _writableRepository = writableRepository;
            _unitOfWork = unitOfWork;
        }

        public override async Task<Result<CommandException<UnregisterFlightCommand>>> HandleAsync(
            UnregisterFlightCommand command, CancellationToken cancellationToken) =>
            await (await _writableRepository.TryFindAsync(command.Id, cancellationToken))
                .Map(async flight =>
                {
                    _writableRepository.Remove(flight);
                    await _writableRepository.SaveAsync(cancellationToken);
                    _unitOfWork.Commit();

                    return Ok;
                })
                .ReduceAsync(ExceptionOf.EntityNotFound(command.Id.ToString()));
    }
}