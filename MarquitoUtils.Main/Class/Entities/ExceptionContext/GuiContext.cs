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
    /// Gui exception
    /// </summary>
    public class GuiContext : ExceptionContext
    {
        /// <summary>
        /// A Gui exception, using when it's a user interface error
        /// </summary>
        /// <param name="title">The title of exception</param>
        /// <param name="message">The message of exception</param>
        /// <param name="action">A custom action</param>
        /// <param name="displayerButton">A displayer button use for the message popup</param>
        public GuiContext(string title, string message, CustomAction action = null, enumDisplayerButton displayerButton = enumDisplayerButton.OK) 
            : base(title, message, action)
        {
            this.Title = title;
            this.Message = message;
            this.DisplayerButton = displayerButton;
            this.Action = action;
            this.exceptionType = enumExceptionType.GUI;
        }
    }
}
