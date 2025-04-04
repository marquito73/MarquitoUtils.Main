using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MarquitoUtils.Main.Class.Enums.EnumLang;

namespace MarquitoUtils.Main.Class.Enums
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
