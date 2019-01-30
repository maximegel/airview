using System.Collections.Generic;
using AirView.Shared.Railways;
using Newtonsoft.Json;

namespace AirView.Api.Core.Hateoas
{
    /// <summary>
    ///     Represents a HAL resource. <see href="https://tools.ietf.org/html/draft-kelly-json-hal-08#section-4" />
    /// </summary>
    [JsonConverter(typeof(LinkedDtoJsonConverter))]
    public class LinkedDto
    {
        public LinkedDto(object state, 
            IReadOnlyDictionary<string, Either<LinkDto, IEnumerable<LinkDto>>> links,
            IReadOnlyDictionary<string, Either<LinkedDto, IEnumerable<LinkedDto>>> embedded)
        {
            State = state;
            Links = links;
            Embedded = embedded;
        }

        /// <summary>
        ///     A dictionary whose property names are link relation types (as defined by [RFC5988]
        ///     (<see href="https://tools.ietf.org/html/rfc5988" />)) and values are either a <see cref="LinkDto" />
        ///     or a sequence of <see cref="LinkDto" />.
        /// </summary>
        public IReadOnlyDictionary<string, Either<LinkDto, IEnumerable<LinkDto>>> Links { get; }

        /// <summary>
        ///     A dictionary whose property names are link relation types (as defined by [RFC5988]
        ///     (<see href="https://tools.ietf.org/html/rfc5988" />)) and values are either a <see cref="LinkedDto" />
        ///     or a sequence of <see cref="LinkedDto" />.
        /// </summary>
        public IReadOnlyDictionary<string, Either<LinkedDto, IEnumerable<LinkedDto>>> Embedded { get; }

        /// <summary>
        ///     The subject resource of the links contained in the <see cref="Links" /> property.
        /// </summary>
        public object State { get; }
    }
}