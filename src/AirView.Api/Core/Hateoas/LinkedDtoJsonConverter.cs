using System;
using System.Collections.Generic;
using System.Linq;
using AirView.Shared.Railways;
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
            var linksToken =
                value.Links
                    .When(links => links.Any())
                    .Map(links => links.ToDictionary(
                        pair => pair.Key,
                        pair => pair.Value.Map(linkList => linkList as object).Reduce(singleLink => singleLink)))
                    .Map(dict => JObject.FromObject(dict, serializer));

            var embeddedToken =
                value.Embedded
                    .When(embedded => embedded.Any())
                    .Map(embedded => embedded.ToDictionary(
                        pair => pair.Key,
                        pair => pair.Value
                            .Map(linkedDtos => WriteJsonArray(linkedDtos, serializer) as JToken)
                            .Reduce(linkedDto => WriteJsonObject(linkedDto, serializer))))
                    .Map(dict => JObject.FromObject(dict, serializer));

            var embeddedProperties = embeddedToken
                .Map(token => token.Properties())
                .Reduce(Enumerable.Empty<JProperty>())
                .Select(prop => prop.Name);

            var properties = JObject
                .FromObject(
                    new {Links = linksToken.Reduce(() => null), Embedded = embeddedToken.Reduce(() => null)},
                    serializer)
                .Properties()
                .Select(prop => new JProperty($"_{prop.Name}", prop.Value))
                .Concat(JObject.FromObject(value.State, serializer)
                    .Properties()
                    .Where(prop => !embeddedProperties.Contains(prop.Name)));

            return new JObject(properties);
        }
    }
}