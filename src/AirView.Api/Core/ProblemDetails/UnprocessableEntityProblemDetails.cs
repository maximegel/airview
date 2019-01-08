using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace AirView.Api.Core.ProblemDetails
{
    public class UnprocessableEntityProblemDetails : Microsoft.AspNetCore.Mvc.ProblemDetails
    {
        public UnprocessableEntityProblemDetails(ModelStateDictionary modelState)
        {
            Title = "One or more validation errors occurred.";
            Status = (int?) HttpStatusCode.UnprocessableEntity;
            Detail = "See errors for details.";
            Errors = new SerializableError(modelState);
        }

        [JsonProperty(Order = 1)]
        public SerializableError Errors { get; }
    }
}