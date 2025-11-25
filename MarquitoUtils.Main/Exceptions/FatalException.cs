using MarquitoUtils.Main.Common.Entities.Action;
using MarquitoUtils.Main.Exceptions.Entities.ExceptionContext;

namespace MarquitoUtils.Main.Exceptions
{
    /// <summary>
    /// A Fatal exception
    /// </summary>
    public class FatalException : Exception
    {
        /// <summary>
        /// A Fatal exception with details about the error
        /// </summary>
        /// <param name="exc">An exception</param>
        public FatalException(Exception exc)
        {
            FatalContext fatalMessage = new FatalContext("Fatal (" + exc.GetType().FullName + ")", exc.Message);
            //FrontAppService frontAppService = new FrontAppServiceImpl();
            //frontAppService.addNewFatal(fatalMessage);
        }

        /// <summary>
        /// A Fatal exception with details about the error, and a custom action
        /// </summary>
        /// <param name="exc">An exception</param>
        /// <param name="action">A custom action</param>
        public FatalException(Exception exc, CustomAction action)
        {
            FatalContext fatalMessage = new FatalContext("Fatal (" + exc.GetType().FullName + ")", exc.Message, action);
            //FrontAppService frontAppService = new FrontAppServiceImpl();
            //frontAppService.addNewFatal(fatalMessage);
        }

        /// <summary>
        /// A Fatal exception with a personalized title and message
        /// </summary>
        /// <param name="title">Title of the fatal</param>
        /// <param name="message">Message of the fatal</param>
        public FatalException(string title, string message)
        {
            FatalContext fatalMessage = new FatalContext("Fatal (" + title + ")", message);
            //FrontAppService frontAppService = new FrontAppServiceImpl();
            //frontAppService.addNewFatal(fatalMessage);
        }

        /// <summary>
        /// A Fatal exception with a personalized title and message, and a custom action
        /// </summary>
        /// <param name="title">Title of the fatal</param>
        /// <param name="message">Message of the fatal</param>
        /// <param name="action">A custom action</param>
        public FatalException(string title, string message, CustomAction action)
        {
            FatalContext fatalMessage = new FatalContext("Fatal (" + title + ")", message, action);
            //FrontAppService frontAppService = new FrontAppServiceImpl();
            //frontAppService.addNewFatal(fatalMessage);
        }
    }
}
