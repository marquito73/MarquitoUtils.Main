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
    public abstract class EnumClass
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
            return GetInheritedEnumTypes<T>(new List<Assembly>());
        }


        /// <summary>
        /// Get all of enums herited of enum class T
        /// </summary>
        /// <typeparam name="T">The class T that the classes must inherit</typeparam>
        /// <param name="otherAssemblies">List of other assemblies for get enum types</param>
        /// <returns>All of enums herited of enum class T</returns>
        public static List<Type> GetInheritedEnumTypes<T>(List<Assembly> otherAssemblies) where T : EnumClass
        {
            // Returned list of enum class types inherited
            List<Type> inheritedEnumTypes = new List<Type>();
            // The type of mother enum class
            Type motherType = typeof(T);
            // Assemblies used to the project
            List<Assembly> assemblies = new List<Assembly> (otherAssemblies);
            assemblies.Add(Assembly.GetEntryAssembly());
            assemblies.Add(Assembly.GetExecutingAssembly());

            foreach (Assembly assembly in assemblies)
            {
                inheritedEnumTypes.AddRange(assembly.GetTypes()
                    .Where(t => t.IsSubclassOf(motherType) || t.Equals(motherType))
                    .Distinct()
                    .ToList());
            }

            return inheritedEnumTypes;
        }

        public static T GetEnumByName<T>(string name) where T : EnumClass
        {
            // The enum
            T enumValue = null;
            // The enum sorted map
            SortedDictionary<string, T> enumMap = GetEnumMap<T>();

            enumValue = enumMap.Where(valPair => valPair.Key == name).First().Value;

            return enumValue;
        }

        public static List<T> GetEnumList<T>() where T : EnumClass
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
        }
    }
}
