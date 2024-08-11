using MarquitoUtils.Main.Class.Entities.Translation;
using MarquitoUtils.Main.Class.Enums;
using MarquitoUtils.Main.Class.Logger;
using MarquitoUtils.Main.Class.Service.Files;
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
    public class TranslateService : FileService, ITranslateService
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

        public string GetTranslation<T>(string translationKey)
        {
            return this.GetTranslation<T>(translationKey, enumLang.EN);
        }

        public string GetTranslation<T>(string translationKey,
            enumLang language)
        {
            string translationFound;

            IEnumerable<Translation> translations = this.Translations
                .Where(translation => translation.Language.Equals(language))
                .Where(translation => translation.Class.IsEquivalentTo(typeof(T)))
                .Where(translation => translation.TranslationKey.Equals(translationKey));

            if (Utils.IsNotEmpty(translations))
            {
                IEnumerable<Translation> translationsFound = translations
                    .Where(translation => translation.OriginApp.Equals(typeof(T).Assembly.GetName().Name));

                if (Utils.IsEmpty(translationsFound))
                {
                    translationsFound = translations
                    .Where(translation => typeof(T).Assembly.GetName().Name.Contains(translation.OriginApp));
                }

                translationFound = translationsFound
                    .Select(translation => translation.TranslationValue)
                    .FirstOrDefault();
            }
            else
            {
                translationFound = "";
            }

            if (Utils.IsEmpty(translationFound))
            {
                translationFound = translationKey;
            }

            return translationFound;
        }

        public void SaveTranslations(string translationFilePath, List<Translation> translations, string applicationName)
        {
            XDocument translationXml = new XDocument();
            // Root element, with application attribute
            XElement rootElement = new XElement("Translations");
            rootElement.SetAttributeValue("application", applicationName);
            translationXml.Add(rootElement);

            // Loop each languages
            List<enumLang> languages = translations.Select(translation => translation.Language)
                .Distinct().Order().ToList();

            foreach (enumLang language in languages)
            {
                // Language element, with language type attribute
                XElement languageElement = new XElement("Language");
                languageElement.SetAttributeValue("lang", language.ToString());
                rootElement.Add(languageElement);
                // Loop each class
                Dictionary<string, List<Translation>> translationMap = translations
                    .Where(translation => translation.Language.Equals(language)).Distinct()
                    .GroupBy(translation => translation.ClassFullName)
                    .ToDictionary(gr => gr.Key, gr => gr.ToList());

                foreach (KeyValuePair<string, List<Translation>> classTranslations in translationMap)
                {
                    // Class element, with class type attribute
                    XElement classElement = new XElement("Class");
                    classElement.SetAttributeValue("cls", classTranslations.Key);
                    languageElement.Add(classElement);

                    // Loop each translations
                    foreach (Translation translation in classTranslations.Value)
                    {
                        // Translation element, with tag attribute
                        XElement translationElement = new XElement("Translation");
                        translationElement.SetAttributeValue("tag", translation.TranslationKey);
                        translationElement.Value = translation.TranslationValue;
                        classElement.Add(translationElement);
                    }
                }
            }

            translationXml.Save(translationFilePath);
        }

        public List<Translation> GetTranslations(string translationFilePath)
        {
            return this.GetTranslations(translationFilePath, this.GetType().Assembly);
        }

        public List<Translation> GetTranslations(string translationFilePath, Assembly fileAssembly)
        {
            List<Translation> translations = new List<Translation>();

            using (Stream configStream = this.GetFileStreamFromManifest(translationFilePath, fileAssembly))
            {
                // Open the xml file
                XDocument translationXml = XDocument.Load(configStream);

                // Loop of each translation of file
                translations = this.GetTranslations(translationXml, fileAssembly);
            }

            return translations;
        }

        public List<Translation> GetTranslationsFromLocation(string translationFilePath)
        {
            // Open the xml file
            XDocument translationXml = XDocument.Load(translationFilePath);

            // Loop of each translation of file
            return this.GetTranslations(translationXml, Assembly.GetEntryAssembly());
        }

        private List<Translation> GetTranslations(XDocument translationXml, Assembly fileAssembly)
        {
            // List of translations in the translation file
            List<Translation> translations = new List<Translation>();

            foreach (XElement appNode in translationXml.Descendants("Translations"))
            {
                // The application name
                string originApp = appNode.Attribute("application").Value;
                // Loop of each language
                foreach (XElement langNode in appNode.Descendants("Language"))
                {
                    // The language
                    enumLang lang = Enum.Parse<enumLang>(langNode.Attribute("lang").Value);
                    // Loop of each class
                    foreach (XElement clsNode in langNode.Descendants("Class"))
                    {
                        // The full class name
                        string fullClassName = clsNode.Attribute("cls").Value;
                        // Construction with the type and the assembly name
                        /*StringBuilder sbFullCs = new StringBuilder();
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
                        Type cls = Type.GetType(sbFullCs.ToString());*/


                        Type cls = this.GetTranslationType(fileAssembly, clsNode, originApp);

                        if (Utils.IsNull(cls))
                        {
                            /*foreach (Assembly assembly in Assembly.GetEntryAssembly().GetReferencedAssemblies().Select(ass => ass.get)
                            {
                                cls = this.GetTranslationType(assembly, clsNode, originApp);
                            }*/
                        }

                        // Loop of each translation
                        foreach (XElement translationNode in clsNode.Descendants("Translation"))
                        {
                            // The label to translate
                            string label = translationNode.Attribute("tag").Value;
                            // The translation
                            string labelTranslation = translationNode.Value
                                .Replace("\n", "").TrimEnd().TrimStart();
                            Translation translation = new Translation(originApp, lang,
                                cls, fullClassName, label, labelTranslation);
                            translations.Add(translation);
                        }
                    }
                }
            }

            return translations;
        }

        private Type GetTranslationType(Assembly assembly, XElement clsNode, string originApp)
        {
            Type resultType;
            //AppDomain.CurrentDomain.GetAssemblies().Where(assembly => assembly.GetName())

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

            if (assembly.AssemblyHasType(sbFullCs.ToString()))
            {
                resultType = assembly.GetType(sbFullCs.ToString());
            }
            else
            {
                sbFullCs = new StringBuilder();
                sbFullCs.Append(clsNode.Attribute("cls").Value);

                IEnumerable<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(assemblyBis => !assemblyBis.GetName().Name.ToLower().StartsWith("system."))
                    .Where(assemblyBis => !assemblyBis.GetName().Name.ToLower().StartsWith("microsoft."));
                

                resultType = assemblies.Where(assemblyBis => assemblyBis.AssemblyHasType(sbFullCs.ToString()))
                    .Select(assemblyBis => assemblyBis.GetType(sbFullCs.ToString()))
                    .FirstOrDefault();
            }

            return resultType;
        }
    }
}
