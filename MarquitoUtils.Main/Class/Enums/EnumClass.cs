using MarquitoUtils.Main.Class.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MarquitoUtils.Main.Class.Enums
{
    /// <summary>
    /// A general enum class
    /// Translation not work for now
    /// </summary>
    public abstract class EnumClass : Attribute, IEnumAttribute
    {

        /// <summary>
        /// Get a traduction of an enum
        /// </summary>
        /// <param name="enumValue">The enum's name</param>
        /// <returns></returns>
        //public static string getEnumTranslation (string enumValue)
        //{
        //    return Translation.getTranslation(enumValue);
        //}

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
            //List<Assembly> assemblies = new List<Assembly> (otherAssemblies);
            //assemblies.Add(Assembly.GetEntryAssembly());
            //assemblies.Add(Assembly.GetExecutingAssembly());
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

        /*public static T GetEnumByName<T>(string name) where T : EnumClass
        {
            // The enum
            T enumValue = null;
            // The enum sorted map
            SortedDictionary<string, T> enumMap = GetEnumMap<T>();

            enumValue = enumMap.Where(valPair => valPair.Key == name).First().Value;

            return enumValue;
        }*/

        /*public static List<T> GetEnumList<T>() where T : EnumClass
        {
            // The enum list
            List<T> enumList = new List<T>();
            // The enum sorted map
            SortedDictionary<string, T> enumMap = GetEnumMap<T>();

            enumList = enumMap.Values.ToList();

            return enumList;
        }

        public static SortedDictionary<string, T> GetEnumMap<T>() where T : EnumClass
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
        }*/

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


                // Temp enum test map
                /*Dictionary<string, Enum> keyValuePairs = enumType
                    .GetCustomAttributes()*/

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

        //GetEnumByName<EnumSize>(sizeName)

        public static E GetEnumByName<E, A>(string name)
            where E : struct, IConvertible
            where A : EnumClass, IEnumAttribute
        {
            // The enum
            E enumValue = default(E);
            // The enum sorted map
            List<E> enumList = GetEnumList<E, A>();

            //enumValue = enumList.Where(e => Enum.Parse(typeof(E), name)).First().Value;

            return (E) Enum.Parse(typeof(E), name);
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
            //List<T> inheritedEnumTypes = new List<T>();
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
                /*inheritedEnumTypes.AddRange(assembly.GetTypes()
                    .Where(t => t.IsSubclassOf(motherType) || t.IsEquivalentTo(motherType))
                    .Distinct()
                    .ToList());*/

                //fields[1].CustomAttributes.ToList()[1].AttributeType.Equals(typeof(A))

                string assemblyName = assembly.FullName.ToLower();

                Type enumType = typeof(Enum);

                List<Type> testEnum = assembly.GetTypes()
                    .Where(t => t.IsSubclassOf(enumType) || t.IsEquivalentTo(enumType))
                    .Distinct()
                    .ToList();

                string testTemp = "";

                List<Type> testTestEnum = testEnum.Where(e =>
                {
                    return e.GetFields()
                    .Where(attr => Utils.IsNotNull(attr.GetCustomAttribute<A>()))
                    .Any();
                }).ToList();




                /*List<T> test = typeof(T)
                    .GetFields()
                    .Select(x => new
                    {
                        att = x.GetCustomAttributes(false)
                                     .OfType<A>()
                                     .FirstOrDefault(),
                        member = x
                    })
                    .Where(x => Utils.IsNotNull(x.att))
                    .Cast<T>()
                    .ToList();*/

                //Assembly.GetExecutingAssembly().GetName().Name.Equals(assembly.GetName().Name)


                /*typeof(T)
                    .GetFields()
                    .Select()*/
            }    

            return inheritedEnumTypes;
        }

        /*public static Attribute GetEnumAttribute<E>(this E value)
            where E : struct, IConvertible
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            return type.GetField(name)
                .GetCustomAttributes(false)
                .OfType<Attribute>()
                .SingleOrDefault();
        }

        public static List<string> GetValuesOf<E>()
            where E : struct, IConvertible
        {
            return Enum.GetValues(typeof(E)).Cast<E>()
                       .Select(val => GetEnumAttribute<E>(val))
                       .ToList();
        }*/

        /*public static SortedDictionary<string, T> GetEnumMapTest<T, A>()
            where T : struct, IConvertible
            where A : Attribute
        {
            // The enum sorted map
            SortedDictionary<string, T> enumMap = new SortedDictionary<string, T>();
            // The list of enum class types inherited
            List<Type> enumTypes = GetInheritedEnumTypesTest<T, A>();

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
        }*/
    }
}
