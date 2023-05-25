using MarquitoUtils.Main.Class.Enums;
using MarquitoUtils.Main.Class.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarquitoUtils.Main.Class.Logger
{
    /// <summary>
    /// Logger
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Log message as debug
        /// </summary>
        /// <param name="message">Message to log</param>
        public static void Debug(string message)
        {
            LogMessage(message, EnumLoggerLevel.Debug);
        }

        /// <summary>
        /// Log message as info
        /// </summary>
        /// <param name="message">Message to log</param>
        public static void Info(string message)
        {
            LogMessage(message, EnumLoggerLevel.Info);
        }

        /// <summary>
        /// Log message as warning
        /// </summary>
        /// <param name="message">Message to log</param>
        public static void Warn(string message)
        {
            LogMessage(message, EnumLoggerLevel.Warning);
        }

        /// <summary>
        /// Log message as error
        /// </summary>
        /// <param name="message">Message to log</param>
        public static void Error(string message)
        {
            LogMessage(message, EnumLoggerLevel.Error);
        }

        /// <summary>
        /// Log message with log level
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="loggerLevel">Log level</param>
        private static void LogMessage(string message, EnumLoggerLevel loggerLevel)
        {
            StringBuilder sbLog = new StringBuilder();

            sbLog.Append(Utils.GetAsString(DateTime.UtcNow)).Append(" [").Append(loggerLevel.GetCode().PadRight(7, ' '))
                .Append("] : ").Append(message);

            Trace.WriteLine(sbLog.ToString());
        }
    }
}
