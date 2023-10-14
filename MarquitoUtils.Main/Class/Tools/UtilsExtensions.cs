using MarquitoUtils.Main.Class.Entities.Param;

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
    }
}
