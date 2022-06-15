using log4net;

namespace MarquitoUtils.Main.Class.Tools.Logger
{
    public class LoggerHelper
    {
        public static ILog GetLogger<T>() where T : class
        {
            return LogManager.GetLogger(typeof(T));
        }
    }
}
