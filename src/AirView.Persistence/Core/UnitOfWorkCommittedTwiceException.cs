using System;

namespace AirView.Persistence.Core
{
    public class UnitOfWorkCommittedTwiceException : UnitOfWorkCommitException
    {
        public UnitOfWorkCommittedTwiceException(string message) :
            base(message)
        {
        }

        public UnitOfWorkCommittedTwiceException(string message, Exception innerException) :
            base(message, innerException)
        {
        }
    }
}