using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace AirView.Api.Core
{
    public class CollectionDto<T>
    {
        public CollectionDto(IEnumerable<T> values, int totalCount)
        {
            Values = values;
            Count = Values.Count();
            TotalCount = totalCount;
        }

        [JsonProperty(Order = 2)]
        public int Count { get; }

        [JsonProperty(Order = 3)]
        public int TotalCount { get; }

        [JsonProperty(Order = 1)]
        public IEnumerable<T> Values { get; }
    }
}