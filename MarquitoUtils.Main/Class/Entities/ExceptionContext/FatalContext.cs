using MarquitoUtils.Main.Class.Entities.Action;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using static MarquitoUtils.Main.Class.Enums.EnumExceptionType;
using static MarquitoUtils.Main.Class.Enums.EnumDisplayerButton;

namespace MarquitoUtils.Main.Class.Entities.ExceptionContext
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
            this.DisplayerButton = enumDisplayerButton.OK;
            this.Action = action;
            this.exceptionType = enumExceptionType.FATAL;
        }
    }
}
