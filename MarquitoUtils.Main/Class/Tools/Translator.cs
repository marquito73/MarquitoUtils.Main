using MarquitoUtils.Main.Class.Entities.Param;
using MarquitoUtils.Main.Class.Entities.Translation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using static MarquitoUtils.Main.Class.Enums.EnumDataType;
using static MarquitoUtils.Main.Class.Enums.EnumLang;

namespace MarquitoUtils.Main.Class.Tools
{
    public class Translator
    {
        /// <summary>
        /// Read the translations
        /// </summary>
        /// <param name="translationFilePath">The file path of the translation</param>
        public static void readTranslationDataFromFile(string translationFilePath)
        {
            readTranslationDataFromFile(translationFilePath, null);
        }
        /// <summary>
        /// Read the translations with specified assembly for the type
        /// </summary>
        /// <param name="translationFilePath">The file path of the translation</param>
        /// <param name="assembly">The assembly</param>
        public static void readTranslationDataFromFile(string translationFilePath, Assembly assembly)
        {
            // List of translations in the translation file
            /*List<Translation> translations =
                (List<Translation>)AppDataPropManage.getValue((int)enumDataType.TRANSLATION_LIST);*/

            List<Translation> translations = new List<Translation>();

            if (Utils.IsNull(translations)) translations = new List<Translation>();
            // Loop of each translation of file
            XDocument translationXml = XDocument.Load(translationFilePath);
            foreach (XElement appNode in translationXml.Descendants("Translations"))
            {
                // The application name
                string originApp = appNode.Attribute("application").Value;
                // Loop of each language
                foreach (XElement langNode in appNode.Descendants("Language"))
                {
                    // The language
                    enumLang lang = System.Enum.Parse<enumLang>(langNode.Attribute("lang").Value);
                    // Loop of each class
                    foreach (XElement clsNode in langNode.Descendants("Class"))
                    {
                        // Construction with the type and the assembly name
                        StringBuilder sbFullCs = new StringBuilder();
                        sbFullCs.Append(clsNode.Attribute("cls").Value).Append(", ");
                        if (Utils.IsNull(assembly))
                        {
                            // With the application attribute specified with the node "Translations"
                            sbFullCs.Append(originApp);
                        }
                        else
                        {
                            // With the specified assembly
                            sbFullCs.Append(assembly.GetName().Name);
                        }
                        // The class
                        Type cls = Type.GetType(sbFullCs.ToString());
                        // Loop of each translation
                        foreach (XElement translationNode in clsNode.Descendants("Translation"))
                        {
                            // The label to translate
                            string label = translationNode.Attribute("tag").Value;
                            // The translation
                            string labelTranslation = translationNode.Value.Replace("\n", "").TrimEnd().TrimStart();
                            Translation translation = new Translation(originApp, lang, cls, label, labelTranslation);
                            translations.Add(translation);
                        }
                    }
                }
            }
            // Add translations to the cache
            //AppDataPropManage.addDataSpecialValue(translations, enumDataType.TRANSLATION_LIST);
        }

        /// <summary>
        /// Default language used to find a translation
        /// </summary>
        private static readonly enumLang defaultLang = enumLang.EN;

        /// <summary>
        /// Get the translation for a label specified in the cache, or with default language (English) if not found
        /// With the originApp name provided by the assembly of the type
        /// </summary>
        /// <param name="cls">The class</param>
        /// <param name="label">The label to translate</param>
        /// <returns>Translation</returns>
        public static string getText(Type cls, string label)
        {
            string originApp = cls.Assembly.GetName().Name;
            return getText(cls, label, originApp);
        }

        /// <summary>
        /// Get the translation for a label specified in the cache, or with default language (English) if not found
        /// </summary>
        /// <param name="cls">The class</param>
        /// <param name="label">The label to translate</param>
        /// <param name="originApp">The origin app</param>
        /// <returns>Translation</returns>
        public static string getText(Type cls, string label, string originApp)
        {
            //enumLang? lang = (enumLang)AppDataPropManage.getValue((int)enumDataType.LANGUAGE);
            enumLang? lang = enumLang.FR;
            if (Utils.IsNull(lang))
            {
                lang = defaultLang;
            }
            return getText(cls, label, originApp, lang.Value);
        }

        /// <summary>
        /// Get the translation for a label
        /// </summary>
        /// <param name="cls">The class</param>
        /// <param name="label">The label to translate</param>
        /// <param name="originApp">The origin app</param>
        /// <param name="lang">The language</param>
        /// <returns>Translation</returns>
        public static string getText(Type cls, string label, string originApp, enumLang lang)
        {
            string labelTranslation = label;
            // Translations
            /*List<Translation> translations =
                (List<Translation>)AppDataPropManage.getValue((int)enumDataType.TRANSLATION_LIST);*/
            List<Translation> translations = new List<Translation>();
            // Loop in translations for find the correct translation
            if (!Utils.IsNull(translations))
            {
                foreach (Translation translation in translations)
                {
                    if (translation.Cls.Equals(cls)
                        && translation.Label.Equals(label)
                        && translation.Lang.Equals(lang)
                        && translation.OriginApp.Equals(originApp))
                    {
                        labelTranslation = translation.LabelTranslation;
                        break;
                    }
                }
            }

            return labelTranslation;
        }
    }
}
