using MarquitoUtils.Main.Common.Entities.Action;
using MarquitoUtils.Main.Exceptions.Entities.ExceptionContext;
using MarquitoUtils.Main.Exceptions.Enums;

namespace MarquitoUtils.Main.Exceptions
{
    /// <summary>
    /// A Gui exception
    /// </summary>
    public class GuiException : Exception
    {
        /// <summary>
        /// A Gui exception with a personalized title and message
        /// </summary>
        /// <param name="title">Title of the fatal</param>
        /// <param name="message">Message of the fatal</param>
        public GuiException(string title, string message)
        {
            GuiContext guiMessage = new GuiContext(title, message);
            //FrontAppService frontAppService = new FrontAppServiceImpl();
            //frontAppService.addNewGui(guiMessage);
        }

        /// <summary>
        /// A Gui exception with a personalized title and message, and a custom action
        /// </summary>
        /// <param name="title">Title of the fatal</param>
        /// <param name="message">Message of the fatal</param>
        /// <param name="action">A custom action</param>
        public GuiException(string title, string message, CustomAction action)
        {
            GuiContext guiMessage = new GuiContext(title, message, action);
            //FrontAppService frontAppService = new FrontAppServiceImpl();
            //frontAppService.addNewGui(guiMessage);
        }

        /// <summary>
        /// A Gui exception with title and message predefined by enumeration
        /// </summary>
        /// <param name="gui">A gui context</param>
        public GuiException(EnumGui gui)
        {
            GuiContext guiMessage = new GuiContext(gui.Title, gui.Message);
            //FrontAppService frontAppService = new FrontAppServiceImpl();
            //frontAppService.addNewGui(guiMessage);
        }

        /// <summary>
        /// A Gui exception with title and message predefined by enumeration, and a custom action
        /// </summary>
        /// <param name="gui">A gui context</param>
        /// <param name="action">A custom action</param>
        public GuiException(EnumGui gui, CustomAction action)
        {
            GuiContext guiMessage = new GuiContext(gui.Title, gui.Message, action);
            //FrontAppService frontAppService = new FrontAppServiceImpl();
            //frontAppService.addNewGui(guiMessage);
        }
    }
}
