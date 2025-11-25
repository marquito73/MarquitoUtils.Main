using MarquitoUtils.Main.Common.Entities.Action;
using MarquitoUtils.Main.Common.Enums;
using MarquitoUtils.Main.Exceptions.Enums;

namespace MarquitoUtils.Main.Exceptions.Entities.ExceptionContext
{
    /// <summary>
    /// Fatal exception
    /// </summary>
    public class FatalContext : ExceptionContext
    {
        /// <summary>
        /// A Fatal exception, using when the exception isn't handled
        /// </summary>
        /// <param name="title">The title of exception</param>
        /// <param name="message">The message of exception</param>
        /// <param name="action">A custom action</param>
        public FatalContext(string title, string message, CustomAction action = null) : base(title, message, action)
        {
            this.Title = title;
            this.Message = message;
            this.DisplayerButton = DisplayerButton.OK;
            this.Action = action;
            this.exceptionType = ExceptionType.FATAL;
        }
    }
}
