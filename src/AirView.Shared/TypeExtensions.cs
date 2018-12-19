using System;
using System.Linq;

namespace AirView.Shared
{
    public static class TypeExtensions
    {
        public static string GetFriendlyName(this Type type) =>
            !type.IsGenericType
                ? type.Name
                : $"{type.Name.Substring(0, type.Name.IndexOf('`'))}<{string.Join(", ", type.GenericTypeArguments.Select(GetFriendlyName))}>";
    }
}