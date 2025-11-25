namespace MarquitoUtils.Main.Logger.Enums
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
            return loggerLevel.ToString().ToUpper();
        }
    }
}
