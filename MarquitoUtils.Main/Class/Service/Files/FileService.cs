using MarquitoUtils.Main.Class.Entities.File;
using MarquitoUtils.Main.Class.Service.General;
using MarquitoUtils.Main.Class.Tools;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace MarquitoUtils.Main.Class.Service.Files
{
    /// <summary>
    /// File service
    /// </summary>
    public class FileService : DefaultService, IFileService
    {
        protected readonly List<string> TextExtensions = new List<string>()
        {
            "txt",
            "js",
            "css",
        };

        public List<string> GetFilesInDirectory(string directory, string extension = "*")
        {
            return Directory.GetFiles(directory, $"*.{extension}", SearchOption.AllDirectories).ToList();
        }

        public string GetFilePathInDirectory(string directory, string fileName, string extension = "*")
        {
            return this.GetFilesInDirectory(directory, extension)
                .Where(filePath => filePath.Contains(fileName)).First();
        }

        public Stream GetFileStreamFromManifest(string fileName, Assembly? assembly = null)
        {
            if (Utils.IsNull(assembly))
            {
                assembly = Assembly.GetEntryAssembly();
            }

            string manifestFileName = assembly.GetManifestResourceNames()
                .Where(file => file.Contains(fileName.Replace("\\", ".")))
                .FirstOrDefault();

            return assembly.GetManifestResourceStream(manifestFileName);
        }

        public CustomFile GetFileStreamFromManifest(string fileName, string extension, Assembly? assembly = null)
        {
            CustomFile file;

            if (this.TextExtensions.Contains(extension))
            {
                // We can read file as text
                using (StreamReader reader = new StreamReader(this.GetFileStreamFromManifest(fileName, assembly)))
                {
                    file = new CustomFile(fileName, extension, reader.ReadToEnd());
                }
            }
            else
            {
                file = new CustomFile(fileName, extension, Utils.ReadAllBytes(this.GetFileStreamFromManifest(fileName, assembly)));
            }

            return file;
        }

        public T GetDataFromXMLFile<T>(string filename, Assembly? assembly = null)
            where T : class, new()
        {
            T data;

            using (Stream fileStream = this.GetFileStreamFromManifest(filename, assembly))
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(fileStream);

                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (XmlReader reader = new XmlNodeReader(xmlDocument))
                {
                    data = (T)serializer.Deserialize(reader);
                }
            }


            return data;
        }

        public DatabaseConfiguration GetDefaultDatabaseConfiguration(Assembly? assembly = null)
        {
            return this.GetDatabaseConfiguration(@"Files\Configuration\Database.config", assembly);
        }

        public DatabaseConfiguration GetDatabaseConfiguration(string databaseConfigurationFileName, Assembly? assembly = null)
        {
            DatabaseConfiguration databaseConfiguration;

            using (Stream configStream = this.GetFileStreamFromManifest(databaseConfigurationFileName, assembly))
            {
                XDocument configFile = XDocument.Load(configStream);

                IEnumerable<XElement> databaseNodes = configFile.Descendants("Configuration").First().Descendants();

                XElement databaseNode;

                if (databaseNodes.Any(elem => elem.Name.LocalName == "Connection"))
                {
                    databaseNode = databaseNodes.Where(elem => elem.Name == "Connection").First();

                    databaseConfiguration = new DatabaseConfiguration(
                        databaseNode.Attribute("User").Value,
                        databaseNode.Attribute("Password").Value,
                        databaseNode.Attribute("ServerName").Value,
                        databaseNode.Attribute("InstanceName").Value,
                        databaseNode.Attribute("Database").Value);
                }
                else
                {
                    databaseNode = databaseNodes.Where(elem => elem.Name == "ConnectionString").First();

                    databaseConfiguration = new DatabaseConfiguration(databaseNode.Attribute("ConnectionString").Value);
                }
            }

            return databaseConfiguration;
        }

        public void SetCreationAndUpdateDateFileProperty(string fileName, DateTime creationDate, DateTime updateDate)
        {
            FileInfo fileInfo = new FileInfo(fileName);

            fileInfo.CreationTime = creationDate;
            fileInfo.LastWriteTime = updateDate;
        }

        public byte[] GetFileAsBytes(string fileName)
        {
            return File.ReadAllBytes(fileName);
        }

        public byte[] GetFileAsBytes(string filePath, Assembly assembly)
        {
            StringBuilder completeTranslationFilePath = new StringBuilder();
            completeTranslationFilePath.Append(Directory.GetParent(assembly.Location)
                .Parent.Parent.Parent.ToString()).Append(filePath);

            return this.GetFileAsBytes(completeTranslationFilePath.ToString());
        }
    }
}
