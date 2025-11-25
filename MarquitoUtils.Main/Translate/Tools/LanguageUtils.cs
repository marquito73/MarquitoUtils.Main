using System.Globalization;
using static MarquitoUtils.Main.Translate.Enums.Language.EnumLang;

namespace MarquitoUtils.Main.Translate.Tools
{
    public class LanguageUtils
    {
        /// <summary>
        /// Get the culture about a language as string
        /// </summary>
        /// <param name="language">A language as string</param>
        /// <returns>The culture about a language as string</returns>
        public static CultureInfo GetCultureLanguage(string language)
        {
            switch (language)
            {
                case "GB":
                    language = "en-gb";
                    break;
                case "US":
                    language = "us-gb";
                    break;
            }
            return CultureInfo.GetCultureInfo(language);
        }

        /// <summary>
        /// Get language from culture
        /// </summary>
        /// <param name="culture">Culture</param>
        /// <returns>Language from culture</returns>
        public static LanguageType GetLanguage(CultureInfo culture)
        {
            string languageISOcode = culture.TwoLetterISOLanguageName;

            return Enum.GetValues(typeof(LanguageType))
                .Cast<LanguageType>()
                .Where(lang => lang.ToString().ToUpper().Equals(languageISOcode.ToUpper()))
                .FirstOrDefault(LanguageType.EN);
        }
    }
}
