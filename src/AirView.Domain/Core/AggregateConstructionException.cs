using System;

namespace AirView.Domain.Core
{
    public class AggregateConstructionException : Exception
    {
        public AggregateConstructionException(string message) :
            base(message)
        {
        }

        public AggregateConstructionException(string message, Exception innerException) :
            base(message, innerException)
        {
        }
    }
}