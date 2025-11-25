using MarquitoUtils.Main.Common.Entities.Action;
using MarquitoUtils.Main.Common.Enums;
using MarquitoUtils.Main.Exceptions.Enums;

namespace MarquitoUtils.Main.Exceptions.Entities.ExceptionContext
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
        public GuiContext(string title, string message, CustomAction action = null, DisplayerButton displayerButton = DisplayerButton.OK)
            : base(title, message, action)
        {
            this.Title = title;
            this.Message = message;
            this.DisplayerButton = displayerButton;
            this.Action = action;
            this.exceptionType = ExceptionType.GUI;
        }
    }
}
