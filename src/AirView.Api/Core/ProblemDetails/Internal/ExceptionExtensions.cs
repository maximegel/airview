using System;
using System.Collections.Generic;

namespace AirView.Api.Core.ProblemDetails.Internal
{
    internal static class ExceptionExtensions
    {
        public static IEnumerable<Exception> GetInnerExceptions(this Exception exception)
        {
            var list = new List<Exception>();
            var innerException = exception?.InnerException;

            while (innerException != null)
            {
                list.Add(innerException);
                innerException = innerException.InnerException;
            }

            return list;
        }
    }
}