using System;

namespace AirView.Persistence.Core
{
    public class UnitOfWorkCommitException : PersistenceException
    {
        public UnitOfWorkCommitException(string message) :
            base(message)
        {
        }

        public UnitOfWorkCommitException(string message, Exception innerException) :
            base(message, innerException)
        {
        }
    }
}