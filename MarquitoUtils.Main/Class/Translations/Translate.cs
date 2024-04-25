using MarquitoUtils.Main.Class.Entities.Translation;
using MarquitoUtils.Main.Class.Service.General;
using MarquitoUtils.Main.Class.Tools;
using System.Globalization;
using static MarquitoUtils.Main.Class.Enums.EnumLang;

namespace MarquitoUtils.Main.Class.Translations
{
    /// <summary>
    /// Static class for store translations
    /// </summary>
    public static class Translate
    {
        /// <summary>
        /// The service for the translations
        /// </summary>
        public static ITranslateService TranslateService { get; private set; }

        /// <summary>
        /// Set translations (don't work if translations are already set
        /// </summary>
        /// <param name="translations"></param>
        public static void SetTranslationService(List<Translation> translations)
        {
            if (Utils.IsNull(TranslateService))
            {
                TranslateService = new TranslateService(translations);
            }
        }

        /// <summary>
        /// Get translation for a class and a translation key
        /// </summary>
        /// <typeparam name="T">The class need the translation</typeparam>
        /// <param name="translationKey">The translation key for translate</param>
        /// <returns>The translation</returns>
        public static string GetTranslation<T>(string translationKey)
        {
            return TranslateService.GetTranslation<T>(translationKey);
        }

        /// <summary>
        /// Get translation for a class and a translation key
        /// </summary>
        /// <typeparam name="T">The class need the translation</typeparam>
        /// <param name="translationKey">The translation key for translate</param>
        /// <param name="language">The language for translate</param>
        /// <returns>The translation</returns>
        public static string GetTranslation<T>(string translationKey, enumLang language)
        {
            return TranslateService.GetTranslation<T>(translationKey, language);
        }

        /// <summary>
        /// Get usable language with culture info
        /// </summary>
        /// <param name="culture">Culture info</param>
        /// <returns>Usable language</returns>
        public static enumLang GetLanguageWithCultureInfo(CultureInfo culture)
        {
            return TranslateService.GetLanguageWithCultureInfo(culture);
        }
    }
}
