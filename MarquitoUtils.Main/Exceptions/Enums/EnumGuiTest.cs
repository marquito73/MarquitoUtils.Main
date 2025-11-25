namespace MarquitoUtils.Main.Exceptions.Enums
{
    /// <summary>
    /// Enumeration for custom Gui message
    /// It's just a example of heritage use of EnumGui
    /// </summary>
    public class EnumGuiTest : EnumGui
    {
        /// <summary>
        /// Enumeration for custom Gui message
        /// It's just a example of heritage use of EnumGui
        /// </summary>
        /// <param name="title">Title of the Gui</param>
        /// <param name="message">Message of the Gui</param>
        public EnumGuiTest(string title, string message) : base(title, message)
        {
            this.Title = title;
            this.Message = message;
        }

        /// <summary>
        /// Gui overload example
        /// </summary>
        public static EnumGuiTest guiOverloadExemple
        {
            get
            {
                return new EnumGuiTest("GuiExample",
                    "Example of EnumGui's overload, for make his owns Gui enums");
            }
        }
    }
}
