using MarquitoUtils.Main.Class.Entities.Param;
using MarquitoUtils.Main.Class.Entities.Sql;
using System.Reflection;

namespace MarquitoUtils.Main.Class.Tools
{
    public static class UtilsExtensions
    {
        public static ICollection<Parameter> AddParameter(this ICollection<Parameter> parameters, 
            string parameterName, object parameterValue)
        {
            parameters.Add(new Parameter(parameterName, parameterValue));

            return parameters;
        }

        /// <summary>
        /// Determinate if none elements match the condition
        /// </summary>
        /// <typeparam name="T">Element's type</typeparam>
        /// <param name="collection">Elements to check</param>
        /// <param name="predicate">The condition</param>
        /// <returns>None elements match the condition ?</returns>
        public static bool None<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
        {
            return !collection.Any(predicate);
        }

        /// <summary>
        /// Return the string with his first letter in uppercase
        /// </summary>
        /// <param name="str">The string</param>
        /// <returns>The string with his first letter in uppercase</returns>
        public static string MakeFirstLetterUpperCase(this string str)
        {
            if (Utils.IsNotEmpty(str))
            {
                str = $"{str[..1].ToUpper()}{str[1..]}";
            }

            return str;
        }

        public static string GetDatePart(this DateTime date)
        {
            return date.ToString().Replace("/", "-").Replace(" ", "_").Replace(":", "-");
        }

        public static PropertyInfo GetFirstPropertyOfType<T>(this Type type)
        {
            return type.GetFirstPropertyOfType(typeof(T));
        }

        public static PropertyInfo GetFirstPropertyOfType(this Type type, Type propertyType)
        {
            return type.GetProperties().Where(prop => prop.PropertyType.Equals(propertyType)).First();
        }

        public static bool HasAttribute<T>(this PropertyInfo prop)
            where T : Attribute
        {
            return Utils.IsNotNull(prop.GetCustomAttribute<T>());
        }

        public static bool IsNumericType(this Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsDateTimeType(this Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                type = type.GenericTypeArguments[0];
            }

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.DateTime:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsNullableType(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public static bool IsAnEntityType(this Type type)
        {
            return type.IsSubclassOf(typeof(Entity));
        }

        /// <summary>
        /// Return true if type is a generic list
        /// </summary>
        /// <param name="type">The type to check</param>
        /// <returns>True if type is a generic list</returns>
        public static bool IsGenericCollectionType(this Type type)
        {
            return type.IsGenericType && (
                type.GetGenericTypeDefinition() == typeof(List<>) ||
                type.GetGenericTypeDefinition() == typeof(ICollection<>));
        }

        /// <summary>
        /// Return true if type is a generic set
        /// </summary>
        /// <param name="type">The type to check</param>
        /// <returns>True if type is a generic set</returns>
        public static bool IsGenericSetType(this Type type)
        {
            return type.IsGenericType && (
                type.GetGenericTypeDefinition() == typeof(Microsoft.EntityFrameworkCore.DbSet<>) ||
                type.GetGenericTypeDefinition() == typeof(IQueryable<>));
        }

        public static bool IsGenericDbSetType(this Type type)
        {
            return IsGenericSetType(type)
                && IsAnEntityType(type.GenericTypeArguments[0]);
        }

        public static bool IsGenericDbCollectionType(this Type type)
        {
            return IsGenericCollectionType(type)
                && IsAnEntityType(type.GenericTypeArguments[0]);
        }

        public static bool AssemblyHasType<T>(this Assembly assembly)
        {
            return assembly.GetTypes().Any(type => type.Equals(typeof(T)));
        }

        public static bool AssemblyHasType(this Assembly assembly, string typeFullName)
        {
            return assembly.GetTypes().Any(type => type.FullName.Equals(typeFullName));
        }

        public static Dictionary<K, V> ToDictionnary<K,V>(this IEnumerable<KeyValuePair<K,V>> mapList)
        {
            return mapList.ToDictionary(x => x.Key, x => x.Value);
        }

        #region Methods for list

        public static void ForEach<T>(this T[] array, Action<T> action)
        {
            array.ToList().ForEach(action);
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            enumerable.ToList().ForEach(action);
        }

        public static void ForEach<T>(this Array array, Action<T> action)
        {
            array.Cast<T>().ToList().ForEach(action);
        }

        public static string Join(this IEnumerable<string> strings, string separator)
        {
            return string.Join(separator, strings);
        }

        #endregion Methods for list
    }
}
