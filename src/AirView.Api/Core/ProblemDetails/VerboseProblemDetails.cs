using System;
using System.Linq;
using AirView.Api.Core.ProblemDetails.Internal;
using AirView.Shared;
using Newtonsoft.Json;

namespace AirView.Api.Core.ProblemDetails
{
    public class VerboseProblemDetails : Microsoft.AspNetCore.Mvc.ProblemDetails
    {
        public VerboseProblemDetails(Exception exception)
        {
            Detail = string.Join(" ---> ", new[] {exception}
                .Concat(exception.GetInnerExceptions())
                .Select(ex => $"{ex.GetType().GetFriendlyName()}: {ex.Message.Replace("\r\n", " ").Trim()}"));
            StackTrace = string.Join("\r\n--- End of inner exception stack trace ---\r\n", new[] {exception}
                    .Concat(exception.GetInnerExceptions())
                    .Reverse()
                    .Select(ex => ex.StackTrace))
                .Split("\r\n")
                .Select(line => line.Trim())
                .ToArray();
        }

        [JsonProperty(Order = 1)]
        public string[] StackTrace { get; }
    }
}