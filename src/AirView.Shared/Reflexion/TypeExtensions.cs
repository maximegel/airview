using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AirView.Shared.Reflexion
{
    public static class TypeExtensions
    {
        /// <summary>
        ///     Returns every base types of the current type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetBases(this Type type)
        {
            do
            {
                yield return type;
                type = type.BaseType;
            } while (type != null);
        }

        /// <summary>
        ///     Returns the number of base classes.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int GetDerivationLevel(this Type type) =>
            type.GetBases().Count() - 1;

        public static bool IsAssignableTo(this Type type, Type otherType)
        {
            var typeInfo = type.GetTypeInfo();
            var otherTypeInfo = otherType.GetTypeInfo();

            return otherTypeInfo.IsGenericTypeDefinition
                ? typeInfo.IsAssignableToGeneric(otherTypeInfo)
                : otherTypeInfo.IsAssignableFrom(typeInfo);
        }

        public static bool IsAssignableToNonGeneric(this TypeInfo type, Type nonGenericType)
        {
            if (type == null)
                return false;

            if (nonGenericType == null)
                return type.IsInterface;

            if (nonGenericType.GetTypeInfo().IsInterface)
                return type.GetInterfaces().Any(
                    @interface => @interface.Name == nonGenericType.Name);

            var currentType = type.BaseType;
            while (currentType != null)
            {
                if (currentType == nonGenericType)
                    return true;
                currentType = currentType.GetTypeInfo().BaseType;
            }
            return false;
        }

        private static bool IsAssignableToGeneric(this TypeInfo typeInfo, Type genericType)
        {
            if (typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition().GetTypeInfo() == genericType)
                return true;

            if (typeInfo.ImplementedInterfaces
                .Select(type => type.GetTypeInfo())
                .Where(interfaceType => interfaceType.IsGenericType)
                .Select(interfaceType => interfaceType.GetGenericTypeDefinition().GetTypeInfo())
                .Any(typeDefinitionTypeInfo => typeDefinitionTypeInfo == genericType))
                return true;

            var baseTypeInfo = typeInfo.BaseType?.GetTypeInfo();
            return baseTypeInfo != null &&
                   baseTypeInfo.IsAssignableToGeneric(genericType);
        }
    }
}