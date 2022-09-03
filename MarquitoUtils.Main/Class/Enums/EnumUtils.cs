using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MarquitoUtils.Main.Class.Enums
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
    }
}
