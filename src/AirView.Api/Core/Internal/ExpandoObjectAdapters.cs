using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace AirView.Api.Core.Internal
{
    internal static class ExpandoObjectAdapters
    {
        public static ExpandoObject Assign(this object obj, object other) =>
            obj.ToExpando().Assign(other);

        public static ExpandoObject ToExpando(this object obj) =>
            (ExpandoObject) obj.GetType().GetProperties().Aggregate(
                (IDictionary<string, object>) new ExpandoObject(),
                (current, property) =>
                {
                    current.Add(property.Name, property.GetValue(obj));
                    return current;
                });
    }
}