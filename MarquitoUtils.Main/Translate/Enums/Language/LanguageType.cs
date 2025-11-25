using MarquitoUtils.Main.Common.Enums;
using static MarquitoUtils.Main.Translate.Enums.Language.EnumLang;

namespace MarquitoUtils.Main.Translate.Enums.Language
{
    /// <summary>
    /// Enumeration regroups languages
    /// </summary>
    public class EnumLang : EnumClass
    {
        /// <summary>
        /// Enumeration regroups languages
        /// </summary>
        public enum LanguageType
        {
            /// <summary>
            /// English
            /// </summary>
            EN,
            /// <summary>
            /// French
            /// </summary>
            FR
        }
    }

    public static class EnumLangExtensions
    {
        public static string GetIsoCountryCode(this LanguageType language)
        {
            string isoCountryCode;

            switch (language)
            {
                case LanguageType.EN:
                    isoCountryCode = "GB";
                    break;
                default:
                    isoCountryCode = language.ToString();
                    break;
            }

            return isoCountryCode;
        }
    }
}
