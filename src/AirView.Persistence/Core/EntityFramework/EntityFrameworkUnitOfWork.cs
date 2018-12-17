using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace AirView.Persistence.Core.EntityFramework
{
    public class EntityFrameworkUnitOfWork<TDbContext> :
        IReadUnitOfWork, IWriteUnitOfWork
        where TDbContext : DbContext
    {
        private readonly TDbContext _context;
        private readonly IDbContextTransaction _transaction;

        public EntityFrameworkUnitOfWork(TDbContext context)
        {
            _context = context;
            _transaction = _context.Database.BeginTransaction();
        }

        public void Dispose() =>
            _context.Dispose();

        public void Commit() =>
            _transaction.Commit();
    }
}