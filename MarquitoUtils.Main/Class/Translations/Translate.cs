using MarquitoUtils.Main.Class.Entities.Translation;
using MarquitoUtils.Main.Class.Service.General;
using MarquitoUtils.Main.Class.Tools;

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
    }
}
