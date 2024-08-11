using MarquitoUtils.Main.Class.Entities.Translation;
using MarquitoUtils.Main.Class.Service.Files;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static MarquitoUtils.Main.Class.Enums.EnumLang;

namespace MarquitoUtils.Main.Class.Service.General
{
    /// <summary>
    /// Translation service
    /// </summary>
    public interface ITranslateService : IFileService
    {
        /// <summary>
        /// Get usable language with culture info
        /// </summary>
        /// <param name="culture">Culture info</param>
        /// <returns>Usable language</returns>
        public enumLang GetLanguageWithCultureInfo(CultureInfo culture);
        /// <summary>
        /// Get translation for a class and a translation key
        /// </summary>
        /// <typeparam name="T">The class need the translation</typeparam>
        /// <param name="translationKey">The translation key for translate</param>
        /// <returns>The translation</returns>
        public string GetTranslation<T>(string translationKey);

        /// <summary>
        /// Get translation for a class and a translation key
        /// </summary>
        /// <typeparam name="T">The class need the translation</typeparam>
        /// <param name="translationKey">The translation key for translate</param>
        /// <param name="language">The language for translate</param>
        /// <returns>The translation</returns>
        public string GetTranslation<T>(string translationKey, 
            enumLang language);

        /// <summary>
        /// Save translations
        /// </summary>
        /// <param name="translationFilePath">The translation file path</param>
        /// <param name="translations">Translations to save</param>
        /// <param name="applicationName">Application name for the translations</param>
        public void SaveTranslations(string translationFilePath, List<Translation> translations, string applicationName);

        /// <summary>
        /// Get translations of translation file
        /// </summary>
        /// <param name="translationFilePath">Translation file path</param>
        /// <returns>Translations of translation file</returns>
        public List<Translation> GetTranslations(string translationFilePath);

        /// <summary>
        /// Get translations of translation file in an assembly
        /// </summary>
        /// <param name="translationFilePath">Translation file path</param>
        /// <param name="fileAssembly">The assembly where the file is located</param>
        /// <returns>Translations of translation file in an assembly</returns>
        public List<Translation> GetTranslations(string translationFilePath, Assembly fileAssembly);

        /// <summary>
        /// Get translations of translation file
        /// </summary>
        /// <param name="translationFilePath">Translation file path</param>
        /// <returns>Translations of translation file</returns>
        public List<Translation> GetTranslationsFromLocation(string translationFilePath);
    }
}
