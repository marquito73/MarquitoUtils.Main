using MarquitoUtils.Main.Class.Entities.Translation;
using MarquitoUtils.Main.Class.Enums;
using MarquitoUtils.Main.Class.Logger;
using MarquitoUtils.Main.Class.Tools;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using static MarquitoUtils.Main.Class.Enums.EnumLang;

namespace MarquitoUtils.Main.Class.Service.General
{
    /// <summary>
    /// Translation service
    /// </summary>
    public class TranslateService : ITranslateService
    {
        /// <summary>
        /// Translations
        /// </summary>
        private List<Translation> Translations { get; set; } = new List<Translation>();

        public TranslateService()
        {

        }

        public TranslateService(List<Translation> Translations)
        {
            this.Translations = Translations;
        }

        public enumLang GetLanguageWithCultureInfo(CultureInfo culture)
        {
            string languageISOcode = culture.TwoLetterISOLanguageName;

            return Enum.GetValues(typeof(enumLang))
                .Cast<enumLang>()
                .Where(lang => lang.ToString().ToUpper().Equals(languageISOcode.ToUpper()))
                .FirstOrDefault(enumLang.EN);
        }

        public string GetTranslation<T>(string translationKey) where T : class
        {
            return this.GetTranslation<T>(translationKey, enumLang.EN);
        }

        public string GetTranslation<T>(string translationKey,
            enumLang language) where T : class
        {
            string translationFound;

            translationFound = this.Translations
                .Where(translation => translation.OriginApp.Equals(typeof(T).Assembly.GetName().Name))
                .Where(translation => translation.Language.Equals(language))
                .Where(translation => translation.Class.IsEquivalentTo(typeof(T)))
                .Where(translation => translation.TranslationKey.Equals(translationKey))
                .Select(translation => translation.TranslationValue)
                .FirstOrDefault();

            if (Utils.IsEmpty(translationFound))
            {
                translationFound = translationKey;
            }

            return translationFound;
        }

        public List<Translation> GetTranslations(string translationFilePath)
        {
            return this.GetTranslations(translationFilePath, this.GetType().Assembly);
        }

        public List<Translation> GetTranslations(string translationFilePath, Assembly fileAssembly)
        {
            // List of translations in the translation file
            List<Translation> translations = new List<Translation>();

            StringBuilder completeTranslationFilePath = new StringBuilder();
            completeTranslationFilePath.Append(Directory.GetParent(fileAssembly.Location)
                .Parent.Parent.Parent.ToString()).Append(translationFilePath);

            if (Utils.IsNull(translations)) translations = new List<Translation>();
            // Loop of each translation of file
            XDocument translationXml = XDocument.Load(completeTranslationFilePath.ToString());
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
                        if (Utils.IsNull(fileAssembly))
                        {
                            // With the application attribute specified with the node "Translations"
                            sbFullCs.Append(originApp);
                        }
                        else
                        {
                            // With the specified assembly
                            sbFullCs.Append(fileAssembly.GetName().Name);
                        }
                        // The class
                        Type cls = Type.GetType(sbFullCs.ToString());
                        // Loop of each translation
                        foreach (XElement translationNode in clsNode.Descendants("Translation"))
                        {
                            // The label to translate
                            string label = translationNode.Attribute("tag").Value;
                            // The translation
                            string labelTranslation = translationNode.Value
                                .Replace("\n", "").TrimEnd().TrimStart();
                            Translation translation = new Translation(originApp, lang,
                                cls, label, labelTranslation);
                            translations.Add(translation);
                        }
                    }
                }
            }

            return translations;
        }
    }
}
