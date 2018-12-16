using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AirView.Shared.Reflexion
{
    public static class MethodInfoSequenceExtensions
    {
        /// <summary>
        ///     Classes the methods of those with the most concrete parameter types
        ///     to those with the less concrete parameter types.
        /// </summary>
        /// <param name="methods"></param>
        /// <returns></returns>
        public static IEnumerable<MethodInfo> OrderByMostDerivedParameters(
            this IEnumerable<MethodInfo> methods) =>
            methods
                .OrderByDescending(method => method.GetParameters()
                    .Select(parameter =>
                        parameter
                            .ParameterType.GetTypeInfo()
                            .GetDerivationLevel())
                    .Sum());
    }
}