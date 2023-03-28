using System;
using System.Collections.Generic;
using System.Text;
using static MarquitoUtils.Main.Class.Enums.EnumLang;

namespace MarquitoUtils.Main.Class.Entities.Translation
{
    public class Translation
    {
        /// <summary>
        /// The origin application for the translations
        /// </summary>
        public string OriginApp { get; set; }

        /// <summary>
        /// Language of translation
        /// </summary>
        public enumLang Language { get; set; }

        /// <summary>
        /// Class of translation
        /// </summary>
        public Type Class { get; set; }

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

        public Translation(string originApp, enumLang language, Type cls, 
            string translationKey, string translationValue)
        {
            this.OriginApp = originApp;
            this.Language = language;
            this.Class = cls;
            this.TranslationKey = translationKey;
            this.TranslationValue = translationValue;
        }
    }
}
