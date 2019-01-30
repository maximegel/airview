using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AirView.Shared
{
    public static class TypeTranslator
    {
        private static readonly Dictionary<Type, string> DefaultTranslations = new Dictionary<Type, string>
        {
            {typeof(int), "int"},
            {typeof(uint), "uint"},
            {typeof(long), "long"},
            {typeof(ulong), "ulong"},
            {typeof(short), "short"},
            {typeof(ushort), "ushort"},
            {typeof(byte), "byte"},
            {typeof(sbyte), "sbyte"},
            {typeof(bool), "bool"},
            {typeof(float), "float"},
            {typeof(double), "double"},
            {typeof(decimal), "decimal"},
            {typeof(char), "char"},
            {typeof(string), "string"},
            {typeof(object), "object"},
            {typeof(void), "void"}
        };

        public static string GetFriendlyName(this Type type, Dictionary<Type, string> translations)
        {
            if (translations.ContainsKey(type))
                return translations[type];
            if (type.IsArray)
                return $"{GetFriendlyName(type.GetElementType(), translations)}[]";
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                return $"{type.GetGenericArguments()[0].GetFriendlyName()}?";
            return type.IsGenericType
                ? $"{type.Name.Split('`')[0]}<{string.Join(", ", type.GetGenericArguments().Select(GetFriendlyName).ToArray())}>"
                : type.Name;
        }

        public static string GetFriendlyName(this Type type) =>
            type.GetFriendlyName(DefaultTranslations);

        public static string GetFriendlySignature(this ConstructorInfo ctor) =>
            $"{ctor.DeclaringType.GetFriendlyName()}({string.Join(", ", ctor.GetParameters().Select(param => param.ParameterType.GetFriendlyName()))})";
    }
}