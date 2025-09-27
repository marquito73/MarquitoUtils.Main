using static MarquitoUtils.Main.Class.Enums.EnumLang;

namespace MarquitoUtils.Main.Class.Entities.Translation
{
    /// <summary>
    /// A translation
    /// </summary>
    //[DebuggerDisplay("Language = {Language}, Class = {ClassFullName}, Key = {TranslationKey}, Value = {TranslationValue}")]
    public class Translation
    {
        /// <summary>
        /// The origin application for the translations
        /// </summary>
        public string OriginApp { get; set; }

        /// <summary>
        /// Language of translation
        /// </summary>
        public LanguageType Language { get; set; }

        /// <summary>
        /// Class of translation
        /// </summary>
        public Type Class { get; set; }
        /// <summary>
        /// Full name of the class for this translation
        /// </summary>
        public string ClassFullName { get; set; }

        /// <summary>
        /// The translation key
        /// </summary>
        public string TranslationKey { get; set; }

        /// <summary>
        /// The label translation
        /// </summary>
        public string TranslationValue { get; set; }

        public Translation()
        {

        }

        /// <summary>
        /// A translation
        /// </summary>
        /// <param name="originApp">The origin application for the translations</param>
        /// <param name="language">Language of translation</param>
        /// <param name="cls">Class of translation</param>
        /// <param name="fullClassName">Full name of the class for this translation</param>
        /// <param name="translationKey">The translation key</param>
        /// <param name="translationValue">The label translation</param>
        public Translation(string originApp, LanguageType language, Type cls, string fullClassName,
            string translationKey, string translationValue)
        {
            this.OriginApp = originApp;
            this.Language = language;
            this.Class = cls;
            this.ClassFullName = fullClassName;
            this.TranslationKey = translationKey;
            this.TranslationValue = translationValue;
        }
    }
}
