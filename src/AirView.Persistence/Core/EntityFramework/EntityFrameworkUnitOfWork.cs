using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace AirView.Persistence.Core.EntityFramework
{
    public sealed class EntityFrameworkUnitOfWork<TDbContext> :
        IReadUnitOfWork, IWriteUnitOfWork
        where TDbContext : DbContext
    {
        private readonly IDbContextTransaction _transaction;

        public EntityFrameworkUnitOfWork(TDbContext context) => 
            _transaction = context.Database.CurrentTransaction ?? context.Database.BeginTransaction();

        public void Dispose()
        {
        }

        public void Commit() =>
            _transaction.Commit();
    }
}