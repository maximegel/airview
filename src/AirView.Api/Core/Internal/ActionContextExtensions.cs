using System.Collections.Generic;
using System.Linq;
using AirView.Shared.Railways;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AirView.Api.Core.Internal
{
    internal static class ActionContextExtensions
    {
        public static IEnumerable<ModelError> GetBodyErrors(this ActionContext context) =>
            !context.ModelState.ContainsKey("")
                ? Enumerable.Empty<ModelError>()
                : context.ModelState[""].Errors;

        public static IEnumerable<ModelError> GetPathErrors(this ActionContext context) =>
            context.ActionDescriptor.Parameters
                .Where(param => param.BindingInfo.BindingSource.Id == "Path")
                .Select(param => context.ModelState[param.Name])
                .Where(modelStateEntry => modelStateEntry.ValidationState == ModelValidationState.Invalid)
                .SelectMany(modelStateEntry => modelStateEntry.Errors);

        public static Option<ModelError> GetUnsupportedContentTypeError(this ActionContext context) =>
            context.ModelState.Values
                .SelectMany(modelState => modelState.Errors)
                .TryFirst(modelError => modelError?.Exception?.GetType() == typeof(UnsupportedContentTypeException));
    }
}