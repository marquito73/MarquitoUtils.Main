using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarquitoUtils.Main.Class.Enums
{
    /// <summary>
    /// Log level enum
    /// </summary>
    public enum EnumLoggerLevel
    {
        /// <summary>
        /// Debug level
        /// </summary>
        Debug,
        /// <summary>
        /// Debug level
        /// </Info>
        Info,
        /// <summary>
        /// Warning level
        /// </summary>
        Warning,
        /// <summary>
        /// Error level
        /// </summary>
        Error,
    }

    /// <summary>
    /// Methods for log level
    /// </summary>
    public static class LoggerMethods
    {
        /// <summary>
        /// Get code for log level
        /// </summary>
        public static string GetCode(this EnumLoggerLevel loggerLevel)
        {
            return loggerLevel.ToString().ToUpper(); ;
        }
    }
}
