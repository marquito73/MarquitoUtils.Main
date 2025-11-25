using MarquitoUtils.Main.Common.Services;
using MarquitoUtils.Main.Files.Entities;
using MarquitoUtils.Main.Sql.Entities;
using System.Reflection;

namespace MarquitoUtils.Main.Files.Services
{
    /// <summary>
    /// File service
    /// </summary>
    public interface IFileService : DefaultService
    {
        /// <summary>
        /// Get files in directory
        /// </summary>
        /// <param name="directory">The directory</param>
        /// <param name="extension">Extension of files searched</param>
        /// <returns>Files in directory</returns>
        public List<string> GetFilesInDirectory(string directory, string extension = "*");

        /// <summary>
        /// Get the file path of file inside directory
        /// </summary>
        /// <param name="directory">The directory</param>
        /// <param name="fileName">The name of the file</param>
        /// <param name="extension">Extension of files searched</param>
        /// <returns>File path of file inside directory</returns>
        public string GetFilePathInDirectory(string directory, string fileName, string extension = "*");

        /// <summary>
        /// Get a stream for a file be in the manifest (file as embedded resource)
        /// </summary>
        /// <param name="fileName">The file name</param>
        /// <param name="assembly">The assembly (if null, take the entry assembly)</param>
        /// <returns>A stream for a file be in the manifest (file as embedded resource)</returns>
        public Stream GetFileStreamFromManifest(string fileName, Assembly? assembly = null);

        public CustomFile GetFileStreamFromManifest(string fileName, string extension, Assembly? assembly = null);

        /// <summary>
        /// Get typed object from an XML file
        /// </summary>
        /// <typeparam name="T">The type of the object returned</typeparam>
        /// <param name="filename">The XML filename</param>
        /// <returns>Typed object from an XML file</returns>
        public T GetDataFromXMLFile<T>(string filename, Assembly? assembly = null) where T : class, new();

        /// <summary>
        /// Get the database configuration file of the project launch the application
        /// </summary>
        /// <returns>The database configuration file of the project launch the application</returns>
        public DatabaseConfiguration GetDefaultDatabaseConfiguration(Assembly? assembly = null);

        /// <summary>
        /// Get the database configuration file of the project launch the application
        /// </summary>
        /// <param name="databaseConfigurationFileName">The database configuration file name</param>
        /// <returns>The database configuration file of the project launch the application</returns>
        public DatabaseConfiguration GetDatabaseConfiguration(string databaseConfigurationFileName, Assembly? assembly = null);

        /// <summary>
        /// Set creation and update date of a file
        /// </summary>
        /// <param name="fileName">The filename</param>
        /// <param name="creationDate">The creation date</param>
        /// <param name="updateDate">The update date</param>
        public void SetCreationAndUpdateDateFileProperty(string fileName, DateTime creationDate, DateTime updateDate);

        public byte[] GetFileAsBytes(string fileName);

        public byte[] GetFileAsBytes(string filePath, Assembly assembly);
    }
}
