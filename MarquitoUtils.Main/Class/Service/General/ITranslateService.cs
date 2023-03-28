using MarquitoUtils.Main.Class.Entities.Translation;
using System;
using System.Collections.Generic;
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
    public interface ITranslateService : DefaultService
    {
        /// <summary>
        /// Get translation for a class and a translation key
        /// </summary>
        /// <typeparam name="T">The class need the translation</typeparam>
        /// <param name="translationKey">The translation key for translate</param>
        /// <returns>The translation</returns>
        public string GetTranslation<T>(string translationKey) where T : class;

        /// <summary>
        /// Get translation for a class and a translation key
        /// </summary>
        /// <typeparam name="T">The class need the translation</typeparam>
        /// <param name="translationKey">The translation key for translate</param>
        /// <param name="language">The language for translate</param>
        /// <returns>The translation</returns>
        public string GetTranslation<T>(string translationKey, 
            enumLang language) where T : class;

        public List<Translation> GetTranslations(string translationFilePath);

        public List<Translation> GetTranslations(string translationFilePath, Assembly fileAssembly);
    }
}
