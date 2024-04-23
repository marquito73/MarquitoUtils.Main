using MarquitoUtils.Main.Class.Entities.File;
using MarquitoUtils.Main.Class.Service.General;
using System.Reflection;
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

        public DatabaseConfiguration GetDefaultDatabaseConfiguration()
        {
            return this.GetDatabaseConfiguration(@"Files\Configuration\Database.config");
        }

        public DatabaseConfiguration GetDatabaseConfiguration(string databaseConfigurationFileName)
        {
            DatabaseConfiguration databaseConfiguration;

            string fileName = Assembly.GetEntryAssembly().GetManifestResourceNames()
                .Where(file => file.Contains(databaseConfigurationFileName.Replace("\\", ".")))
                .FirstOrDefault();

            using (Stream configStream = Assembly.GetEntryAssembly().GetManifestResourceStream(fileName))
            {
                XDocument configFile = XDocument.Load(configStream);

                XElement databaseNode = configFile.Descendants("Configuration").First().Descendants("Connection").First();

                databaseConfiguration = new DatabaseConfiguration(
                    databaseNode.Attribute("User").Value,
                    databaseNode.Attribute("Password").Value, 
                    databaseNode.Attribute("ServerName").Value,
                    databaseNode.Attribute("InstanceName").Value,
                    databaseNode.Attribute("Database").Value);
            }

            return databaseConfiguration;
        }

        public void SetCreationAndUpdateDateFileProperty(string fileName, DateTime creationDate, DateTime updateDate)
        {
            FileInfo fileInfo = new FileInfo(fileName);

            fileInfo.CreationTime = creationDate;
            fileInfo.LastWriteTime = updateDate;
        }
    }
}
