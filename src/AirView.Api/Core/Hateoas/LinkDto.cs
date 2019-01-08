using Newtonsoft.Json;

namespace AirView.Api.Core
{
    /// <summary>
    ///     Represents a hyperlink from the containing resource to a URI.
    ///     <see href="https://tools.ietf.org/html/draft-kelly-json-hal-08#section-5" />
    /// </summary>
    public class LinkDto
    {
        public LinkDto(string href) =>
            Href = href;

        /// <summary>
        ///     Either a URI [RFC3986] (<see href="https://tools.ietf.org/html/rfc3986" />) or a URI Template [RFC6570]
        ///     (<see href="https://tools.ietf.org/html/rfc6570" />) relative to the API root pointing that points to the target
        ///     resource or the target action.
        /// </summary>
        /// <remarks>
        ///     If the value is a URI Template then the <see cref="Templated" /> property should be true.
        /// </remarks>
        [JsonProperty(Order = 1)]
        public string Href { get; }

        /// <summary>
        ///     If present, indicates that the link is to be deprecated (i.e. removed) at a future date. The value is a URL that
        ///     should provide further information about the deprecation.
        /// </summary>
        [JsonProperty(Order = 5)]
        public string Deprecation { get; set; }

        /// <summary>
        ///     The language of the target resource (as defined by [RFC5988]).
        /// </summary>
        [JsonProperty(Order = 8)]
        public string Hreflang { get; set; }

        /// <summary>
        ///     A unique name may be used as a secondary key for selecting links which share the same relation type.
        /// </summary>
        [JsonProperty(Order = 6)]
        public string Name { get; set; }

        /// <summary>
        ///     A URI that hints about the profile of the target resource (as defined by [I-D.wilde-profile-link]
        ///     (<see href="https://tools.ietf.org/html/draft-kelly-json-hal-08#ref-I-D.wilde-profile-link" />)).
        /// </summary>
        [JsonProperty(Order = 7)]
        public string Profile { get; set; }

        /// <summary>
        ///     Indicates if the <see cref="Href" /> property is a URI Template.
        /// </summary>
        /// <remarks>
        ///     Should be considered false if it is undefined (i.e. null) or any other value than true.
        /// </remarks>
        [JsonProperty(Order = 2)]
        public bool? Templated { get; set; }

        /// <summary>
        ///     A human-readable identifier intended for labelling the link (as defined by [RFC5988]
        ///     (<see href="https://tools.ietf.org/html/rfc5988" />)).
        /// </summary>
        [JsonProperty(Order = 3)]
        public string Title { get; set; }

        /// <summary>
        ///     The media type expected when dereferencing the target resource.
        /// </summary>
        [JsonProperty(Order = 4)]
        public string Type { get; set; }
    }
}