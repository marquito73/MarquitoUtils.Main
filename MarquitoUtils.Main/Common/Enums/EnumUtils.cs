using MarquitoUtils.Main.Common.Tools;
using System.Reflection;

namespace MarquitoUtils.Main.Common.Enums
{
    public static class EnumUtils
    {
        public static A GetAttr<A, T>(this T contentType)
            where T : struct, IConvertible
            where A : EnumClass, IEnumAttribute
        {
            return (A)Attribute.GetCustomAttribute(ForValue(contentType),
                typeof(A));
        }

        public static MemberInfo ForValue<T>(this T contentType)
            where T : struct, IConvertible
        {
            return typeof(T).GetField(Enum.GetName(typeof(T), contentType));
        }

        public static TEnum GetEnum<TEnum>(string enumName)
            where TEnum : struct, IConvertible
        {
            return (TEnum)Enum.Parse(typeof(TEnum), enumName);
        }

        public static string GetEnumName<TEnum>(this TEnum enumeration)
            where TEnum : struct, IConvertible
        {
            return enumeration.ToString();
        }

        public static List<TEnum> GetList<TEnum>()
            where TEnum : struct, Enum
        {
            return Enum.GetValues<TEnum>().ToList();
        }

        /// <summary>
        /// Get the attribute of an enum value, it returns null if the attribute is not found
        /// </summary>
        /// <typeparam name="TAttr">The attribute to found</typeparam>
        /// <typeparam name="TEnum">The enum type to search</typeparam>
        /// <param name="enumValue">The specific enum to search</param>
        /// <returns>The attribute founded</returns>
        public static TAttr? GetAttribute<TAttr, TEnum>(this TEnum enumValue)
            where TAttr : Attribute
            where TEnum : struct, IConvertible
        {
            return typeof(TEnum).GetFields()
                    .Where(field => field.Name.Equals(enumValue.ToString()))
                    .Where(field => field.HasAttribute<TAttr>())
                    .FirstOrDefault()?.GetCustomAttribute<TAttr>();

        }
    }
}
