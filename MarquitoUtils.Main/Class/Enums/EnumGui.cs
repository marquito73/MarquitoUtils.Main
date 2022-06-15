using System;
using System.Collections.Generic;
using System.Text;

namespace MarquitoUtils.Main.Class.Enums
{
    /// <summary>
    /// Enumeration for custom Gui message
    /// Can be herited for define and use others messages
    /// </summary>
    public class EnumGui : EnumClass
    {
        /// <summary>
        /// Title of the Gui
        /// </summary>
        public string Title { get; protected set; }

        /// <summary>
        /// Message of the Gui
        /// </summary>
        public string Message { get; protected set; }

        /// <summary>
        /// Enumeration for custom Gui message
        /// Can be herited for define and use others messages
        /// </summary>
        /// <param name="title">Title of the Gui</param>
        /// <param name="message">Message of the Gui</param>
        protected EnumGui(string title, string message) 
        { 
            Title = title;
            Message = message;
        }

        /// <summary>
        /// Gui enum example
        /// </summary>
        public static EnumGui guiExemple
        {
            get
            {
                return new EnumGui("Exemple",
                    "Exemple de EnumGui");
            }
        }
    }
}
