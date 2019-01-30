using System;
using System.Collections.Generic;
using System.Linq;
using AirView.Shared.Railways;
using Humanizer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AirView.Api.Core.Hateoas
{
    public class LinkedDtoJsonConverter : JsonConverter<LinkedDto>
    {
        /// Indicates (in a thread-safe manner) if we should stop writing.
        [ThreadStatic] private static bool _cannotWrite;

        public override bool CanWrite => !_cannotWrite;

        public override LinkedDto ReadJson(JsonReader reader, Type objectType, LinkedDto existingValue,
            bool hasExistingValue, JsonSerializer serializer) =>
            throw new NotImplementedException();

        public override void WriteJson(JsonWriter writer, LinkedDto value, JsonSerializer serializer)
        {
            // We stop writing to prevents infinite recursion.
            _cannotWrite = true;
            WriteJsonObject(value, serializer).WriteTo(writer);
            _cannotWrite = false;
        }

        private static JArray WriteJsonArray(IEnumerable<LinkedDto> values, JsonSerializer serializer) =>
            JArray.FromObject(values.Select(value => WriteJsonObject(value, serializer)), serializer);

        private static JObject WriteJsonObject(LinkedDto value, JsonSerializer serializer)
        {
            var obj = new JObject();

            if (value.Links.Any())
                obj.Add("_links", JObject.FromObject(
                    value.Links.ToDictionary(
                        // TODO(maximegelinas): Find a better way.
                        pair => pair.Key.Camelize(),
                        pair => pair.Value
                            .Map(links => JArray.FromObject(links, serializer) as JToken)
                            .Reduce(link => JObject.FromObject(link, serializer)))));

            foreach (var property in JObject.FromObject(value.State, serializer).Properties()) obj.Add(property);

            if (!value.Embedded.Any())
                return obj;

            var embeddedObj = JObject.FromObject(
                value.Embedded.ToDictionary(
                    // TODO(maximegelinas): Find a better way.
                    pair => pair.Key.Camelize(),
                    pair => pair.Value
                        .Map(linkedDtos => WriteJsonArray(linkedDtos, serializer) as JToken)
                        .Reduce(linkedDto => WriteJsonObject(linkedDto, serializer))));
            obj.Add("_embedded", embeddedObj);

            foreach (var property in embeddedObj.Properties()) obj.Remove(property.Name);

            return obj;
        }
    }
}