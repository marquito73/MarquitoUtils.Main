namespace MarquitoUtils.Main.Common.Enums
{
    /// <summary>
    /// Enum define data keys used in AppDataPropManager
    /// Negative keys for instant find
    /// </summary>
    public class EnumDataType : EnumClass
    {
        /// <summary>
        /// Enum define data keys used in AppDataPropManager
        /// Negative keys for instant find
        /// </summary>
        public enum enumDataType : int
        {
            UNDEFINED = -1,
            THREAD_LIST = -2,
            EXCEPTION_LIST = -3,
            CONTROL_LIST = -4,
            CONF_PARAM_LIST = -5,
            TRANSLATION_LIST = -6,
            LANGUAGE = -7,
            PIPE_OBJECT = -8
        }

        /// <summary>
        /// Get a list of all enums
        /// </summary>
        /// <returns>List of all enums</returns>
        public static List<enumDataType> getEnumDataTypeList()
        {
            List<enumDataType> lstEnumDataType = System.Enum.GetValues(typeof(enumDataType)).Cast<enumDataType>().ToList();

            return lstEnumDataType;
        }
    }
}
