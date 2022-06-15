using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarquitoUtils.Main.Class.Enums
{
    /// <summary>
    /// Enum define if we use server or client pipemode
    /// </summary>
    public class EnumPipeMode : EnumClass
    {
        /// <summary>
        /// Enum define if we use server or client pipemode
        /// </summary>
        public enum enumPipeMode : int
        {
            SERVER = 0,
            CLIENT = 1
        }

        /// <summary>
        /// Get a list of all enums
        /// </summary>
        /// <returns>List of all enums</returns>
        public static List<enumPipeMode> getEnumPipeModeList()
        {
            List<enumPipeMode> lstEnumPipeMode = System.Enum.GetValues(typeof(enumPipeMode)).Cast<enumPipeMode>().ToList();

            return lstEnumPipeMode;
        }
    }
}
