using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using AirView.Shared.Railways;

namespace AirView.Api.Core.Hateoas
{
    public static class DtoExtensions
    {
        private static readonly ConditionalWeakTable<object, Dictionary<string, Either<LinkDto, IEnumerable<LinkDto>>>>
            Links = new ConditionalWeakTable<object, Dictionary<string, Either<LinkDto, IEnumerable<LinkDto>>>>();

        public static void AddLink(this object dto, string rel, LinkDto link) =>
            Links.GetOrCreateValue(dto).Add(rel, link);

        public static LinkedDto ToLinkedDto(this object dto)
        {
            var embeddedDtos = dto.GetType()
                .GetProperties()
                .Select(property =>
                {
                    var rel = property.Name;
                    var value = property.GetValue(dto);

                    if (!(value is IEnumerable values) || value is string)
                        return !HasLinks(value)
                            ? Option.None
                            : Option.Some(
                                (rel, linkedDto: Either<LinkedDto, IEnumerable<LinkedDto>>.From(value.ToLinkedDto())));

                    var linkedDtos = values
                        .Cast<object>()
                        .Select(val => !HasLinks(val) ? Option.None : Option.Some(val.ToLinkedDto()))
                        .Flatten()
                        .ToArray();

                    return Option.Some((rel, linkedDto: Either<LinkedDto, IEnumerable<LinkedDto>>.From(linkedDtos)));
                })
                .Flatten()
                .ToDictionary(tuple => tuple.rel, tuple => tuple.linkedDto);

            return new LinkedDto(dto, Links.GetOrCreateValue(dto), embeddedDtos);
        }

        internal static bool HasLinks(this Type dtoType) =>
            Links.AsEnumerable().Any(pair => dtoType.IsInstanceOfType(pair.Key));

        internal static bool HasLinks(this object dto) =>
            dto != null && Links.TryGetValue(dto, out _);
    }
}