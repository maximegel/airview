using System;
using System.Linq;
using System.Reflection;

namespace AirView.Shared.Reflexion
{
    public static class MethodInfoExtensions
    {
        /// <summary>
        ///     Returns true if the method can be invoke with the given signature.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="parametersTypes"></param>
        /// <returns></returns>
        public static bool IsInvokableWith(
            this MethodInfo method, params Type[] parametersTypes) =>
            method.GetParameters().Length == parametersTypes.Length &&
            method.GetParameters()
                .Select(param => param.ParameterType)
                .Zip(parametersTypes, (param, otherParam) => param.IsAssignableTo(otherParam))
                .All(match => match);
    }
}