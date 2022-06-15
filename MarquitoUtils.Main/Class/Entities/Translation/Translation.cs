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
        public enumLang Lang { get; set; }

        /// <summary>
        /// Class of translation
        /// </summary>
        public Type Cls { get; set; }

        /// <summary>
        /// The label to translate
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// The label translation
        /// </summary>
        public string LabelTranslation { get; set; }

        public Translation()
        {

        }

        public Translation(string originApp, enumLang lang, Type cls, string label, string labelTranslation)
        {
            this.OriginApp = originApp;
            this.Lang = lang;
            this.Cls = cls;
            this.Label = label;
            this.LabelTranslation = labelTranslation;
        }
    }
}
