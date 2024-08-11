using MarquitoUtils.Main.Class.Entities.File;
using MarquitoUtils.Main.Class.Service.General;
using MarquitoUtils.Main.Class.Tools;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace MarquitoUtils.Main.Class.Service.Files
{
    /// <summary>
    /// File service
    /// </summary>
    public class FileService : DefaultService, IFileService
    {
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

        public DatabaseConfiguration GetDefaultDatabaseConfiguration()
        {
            return this.GetDatabaseConfiguration(@"Files\Configuration\Database.config");
        }

        public DatabaseConfiguration GetDatabaseConfiguration(string databaseConfigurationFileName)
        {
            DatabaseConfiguration databaseConfiguration;

            using (Stream configStream = this.GetFileStreamFromManifest(databaseConfigurationFileName))
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
