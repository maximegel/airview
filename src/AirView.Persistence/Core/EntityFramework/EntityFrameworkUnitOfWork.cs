using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace AirView.Persistence.Core.EntityFramework
{
    public sealed class EntityFrameworkUnitOfWork :
        IReadUnitOfWork, IWriteUnitOfWork
    {
        private readonly IDbContextTransaction _transaction;

        public EntityFrameworkUnitOfWork(DbContext context) =>
            _transaction = context.Database.CurrentTransaction ?? context.Database.BeginTransaction();

        public void Dispose()
        {
        }

        public void Commit() =>
            _transaction.Commit();
    }
}