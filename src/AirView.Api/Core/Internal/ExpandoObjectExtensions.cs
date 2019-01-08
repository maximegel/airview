using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace AirView.Api.Core.Internal
{
    internal static class ExpandoObjectExtensions
    {
        public static ExpandoObject Assign(this ExpandoObject expando, object other) =>
            Assign(expando, other.ToExpando());

        public static ExpandoObject Assign(this ExpandoObject expando, ExpandoObject other) =>
            (ExpandoObject) other.Aggregate(
                (IDictionary<string, object>) expando,
                (current, property) =>
                {
                    current[property.Key] = property.Value;
                    return current;
                });
    }
}