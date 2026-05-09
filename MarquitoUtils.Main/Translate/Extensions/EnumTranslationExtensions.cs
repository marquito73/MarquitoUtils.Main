using MarquitoUtils.Main.Translate.Services;
using MarquitoUtils.Main.Translate.Tools;
using System.Globalization;
using static MarquitoUtils.Main.Translate.Enums.Language.EnumLang;

namespace MarquitoUtils.Main.Translate.Extensions
{
    /// <summary>
    /// Extensions methods for translating enum values using the ITranslateService.
    /// </summary>
    public static class EnumTranslationExtensions
    {
        /// <summary>
        /// Gets the translation of an enum value using the provided ITranslateService.
        /// </summary>
        /// <typeparam name="TEnum">Enum type</typeparam>
        /// <param name="enumValue">Enum to translate</param>
        /// <param name="translateService">Translation service</param>
        /// <param name="language">Language for translation (default is EN)</param>
        /// <returns></returns>
        public static string GetTranslation<TEnum>(this TEnum enumValue, ITranslateService translateService, LanguageType language = LanguageType.EN)
            where TEnum : struct, IConvertible
        {
            return translateService.GetTranslation<TEnum>(enumValue.ToString(), language);
        }

        /// <summary>
        /// Gets the translation of an enum value using the provided ITranslateService.
        /// The translation is based on the current culture's language.
        /// </summary>
        /// <typeparam name="TEnum">Enum type</typeparam>
        /// <param name="enumValue">Enum to translate</param>
        /// <param name="translateService">Translation service</param>
        /// <returns></returns>
        public static string GetTranslation<TEnum>(this TEnum enumValue, ITranslateService translateService)
            where TEnum : struct, IConvertible
        {
            return enumValue.GetTranslation<TEnum>(translateService, LanguageUtils.GetLanguage(CultureInfo.CurrentCulture));
        }
    }
}
