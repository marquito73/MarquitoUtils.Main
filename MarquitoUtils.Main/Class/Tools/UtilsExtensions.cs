using MarquitoUtils.Main.Class.Entities.Param;
using MarquitoUtils.Main.Class.Entities.Sql;
using NPOI.SS.Formula.Functions;
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

        public static bool AssemblyHasType<T>(this Assembly assembly)
        {
            return assembly.GetTypes().Any(type => type.Equals(typeof(T)));
        }

        public static bool AssemblyHasType(this Assembly assembly, string typeFullName)
        {
            return assembly.GetTypes().Any(type => type.FullName.Equals(typeFullName));
        }
    }
}
