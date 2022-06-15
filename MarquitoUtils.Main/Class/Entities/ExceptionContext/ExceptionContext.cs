﻿using MarquitoUtils.Main.Class.Entities.Action;
using System;
using System.Collections.Generic;
using System.Text;
using static MarquitoUtils.Main.Class.Enums.EnumExceptionType;
using static MarquitoUtils.Main.Class.Enums.EnumDisplayerButton;

namespace MarquitoUtils.Main.Class.Entities.ExceptionContext
{
    /// <summary>
    /// Custom exception
    /// </summary>
    public class ExceptionContext
    {
        /// <summary>
        /// The title of exception
        /// </summary>
        public string Title { get; protected set; }

        /// <summary>
        /// The message of exception
        /// </summary>
        public string Message { get; protected set; }

        /// <summary>
        /// A custom action
        /// </summary>
        public CustomAction Action { get; protected set; }

        /// <summary>
        /// A displayer button use for the message popup
        /// </summary>
        public enumDisplayerButton DisplayerButton { get; protected set; }

        /// <summary>
        /// An exception type
        /// </summary>
        public enumExceptionType exceptionType { get; protected set; }

        /// <summary>
        /// A custom exception
        /// </summary>
        /// <param name="title">The title of exception</param>
        /// <param name="message">The message of exception</param>
        /// <param name="action">A custom action</param>
        public ExceptionContext(string title, string message, CustomAction action = null)
        {
            this.Title = title;
            this.Message = message;
            this.Action = action;
        }
    }
}
