using System;

namespace AirView.Persistence.Core
{
    public class PersistenceException : Exception
    {
        public PersistenceException(string message) :
            base(message)
        {
        }

        public PersistenceException(string message, Exception innerException) :
            base(message, innerException)
        {
        }
    }
}