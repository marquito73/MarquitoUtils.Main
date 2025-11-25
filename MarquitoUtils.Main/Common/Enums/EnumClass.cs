using MarquitoUtils.Main.Common.Tools;
using System.Reflection;

namespace MarquitoUtils.Main.Common.Enums
{
    /// <summary>
    /// A general enum class
    /// Translation not work for now
    /// </summary>
    public abstract class EnumClass : Attribute, IEnumAttribute
    {

        public EnumClass()
        {

        }


        /// <summary>
        /// Get all of enums herited of enum class T
        /// </summary>
        /// <typeparam name="T">The class T that the classes must inherit</typeparam>
        /// <returns>All of enums herited of enum class T</returns>
        public static List<Type> GetInheritedEnumTypes<T>() where T : EnumClass
        {
            // Returned list of enum class types inherited
            List<Type> inheritedEnumTypes = new List<Type>();
            // The type of mother enum class
            Type motherType = typeof(T);
            // Assemblies used in the entire project
            List<Assembly> assemblies = new List<Assembly>();

            assemblies.AddRange(AppDomain.CurrentDomain.GetAssemblies().ToList());

            foreach (Assembly assembly in assemblies)
            {
                inheritedEnumTypes.AddRange(assembly.GetTypes()
                    .Where(t => t.IsSubclassOf(motherType) || t.IsEquivalentTo(motherType))
                    .Distinct()
                    .ToList());
            }

            return inheritedEnumTypes;
        }

        public static SortedDictionary<string, T> GetEnumMapTest<T>()
            where T : EnumClass
        {
            // The enum sorted map
            SortedDictionary<string, T> enumMap = new SortedDictionary<string, T>();
            // The list of enum class types inherited
            List<Type> enumTypes = GetInheritedEnumTypes<T>();

            foreach (Type enumType in enumTypes)
            {
                // Temp properties enum map
                Dictionary<string, T> enumMapProperties = enumType
                    .GetProperties(BindingFlags.Public | BindingFlags.Static)
                    .Where(p => p.PropertyType.Equals(enumType))
                    .ToDictionary(p => p.Name, p => (T)p.GetValue(null));
                // Temp fields enum map
                Dictionary<string, T> enumMapFields = enumType
                    .GetFields(BindingFlags.Public | BindingFlags.Static)
                    .Where(f => f.FieldType.Equals(enumType))
                    .ToDictionary(f => f.Name, f => (T)f.GetValue(null));

                var mapTest = enumType
                    .GetCustomAttributes();

                foreach (KeyValuePair<string, T> enumProperty in enumMapProperties)
                {
                    // Add each enum to sorted map
                    enumMap.Add(enumProperty.Key, enumProperty.Value);
                }

                foreach (KeyValuePair<string, T> enumField in enumMapFields)
                {
                    // Add each enum to sorted map
                    enumMap.Add(enumField.Key, enumField.Value);
                }
            }

            return enumMap;
        }

        public static E GetEnumByName<E>(string name)
            where E : struct, Enum, IConvertible
        {
            return (E)Enum.Parse(typeof(E), name);
        }

        public static IEnumerable<E> GetList<E>()
            where E : struct, Enum, IConvertible
        {
            return Enum.GetValues<E>();
        }

        public static List<E> GetEnumList<E, A>()
            where E : struct, IConvertible
            where A : EnumClass, IEnumAttribute
        {
            // The enum list
            List<E> enumList = new List<E>();

            A attribute = typeof(E)
                .GetFields()
                .Select(field => field.GetCustomAttributes())
                .SelectMany(x => x)
                .Distinct()
                .Where(attr => Utils.TypeIsInheritedBy(attr.GetType(), typeof(A)))
                .Cast<A>()
                .First();

            // The enum sorted map
            SortedDictionary<string, List<Enum>> enumMap = GetEnumMap(attribute);

            enumList = enumMap.Values
                .SelectMany(x => x)
                .Cast<E>()
                .ToList();

            return enumList;
        }

        public static SortedDictionary<string, List<Enum>> GetEnumMap<A>(A attr)
            where A : EnumClass, IEnumAttribute
        {
            return GetEnumMap<A>();
        }

        public static SortedDictionary<string, List<Enum>> GetEnumMap<A>()
            where A : EnumClass, IEnumAttribute
        {
            // Returned map of enum class types inherited
            SortedDictionary<string, List<Enum>> inheritedEnumTypes = new SortedDictionary<string, List<Enum>>();
            // The type of mother enum class
            Type motherType = typeof(Enum);
            // Assemblies used in the entire project
            List<Assembly> assemblies = new List<Assembly>();

            assemblies.AddRange(AppDomain.CurrentDomain.GetAssemblies().ToList());

            foreach (Assembly assembly in assemblies)
            {
                List<Type> enums = assembly.GetTypes()
                    .Where(t => Utils.TypeIsInheritedBy(t, motherType))
                    .Distinct()
                    .Where(e =>
                    {
                        return e.GetFields()
                        .Where(attr => Utils.IsNotNull(attr.GetCustomAttribute<A>()))
                        .Any();
                    }).ToList();

                foreach (Type enumType in enums)
                {
                    if (!inheritedEnumTypes.ContainsKey(enumType.Name))
                    {
                        inheritedEnumTypes.Add(enumType.Name, Enum.GetValues(enumType).Cast<Enum>().ToList());
                    }
                }
            }

            return inheritedEnumTypes;
        }

        /// <summary>
        /// Get all of enums herited of enum class T
        /// </summary>
        /// <typeparam name="T">The class T that the classes must inherit</typeparam>
        /// <returns>All of enums herited of enum class T</returns>
        public static List<T> GetInheritedEnumTypesTest<T, A>()
            where T : struct, IConvertible
            where A : EnumClass, IEnumAttribute
        {
            // Returned list of enum class types inherited
            List<T> inheritedEnumTypes = new List<T>();
            // The type of mother enum class
            Type motherType = typeof(T);
            // Assemblies used in the entire project
            List<Assembly> assemblies = new List<Assembly>();

            assemblies.AddRange(AppDomain.CurrentDomain.GetAssemblies().ToList());

            List<Type> attrTypes = typeof(T)
                .GetFields()
                .Select(x => x.CustomAttributes)
                .SelectMany(x => x)
                .Where(x => x.AttributeType.Equals(typeof(A)))
                .Select(x => x.AttributeType)
                .Distinct()
                .ToList();

            foreach (Assembly assembly in assemblies)
            {
                string assemblyName = assembly.FullName.ToLower();

                Type enumType = typeof(Enum);

                List<Type> testEnum = assembly.GetTypes()
                    .Where(t => t.IsSubclassOf(enumType) || t.IsEquivalentTo(enumType))
                    .Distinct()
                    .ToList();

                List<Type> testTestEnum = testEnum.Where(e =>
                {
                    return e.GetFields()
                    .Where(attr => Utils.IsNotNull(attr.GetCustomAttribute<A>()))
                    .Any();
                }).ToList();
            }

            return inheritedEnumTypes;
        }
    }
}
