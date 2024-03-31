using MarquitoUtils.Main.Class.Entities.File;

namespace MarquitoUtils.Main.Class.Service.Files
{
    /// <summary>
    /// File service
    /// </summary>
    public interface IFileService
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
        /// Get the database configuration file of the project launch the application
        /// </summary>
        /// <param name="databaseConfigurationFileName">The database configuration file name</param>
        /// <returns>The database configuration file of the project launch the application</returns>
        public DatabaseConfiguration GetDatabaseConfiguration(string databaseConfigurationFileName);

        /// <summary>
        /// Set creation and update date of a file
        /// </summary>
        /// <param name="fileName">The filename</param>
        /// <param name="creationDate">The creation date</param>
        /// <param name="updateDate">The update date</param>
        public void SetCreationAndUpdateDateFileProperty(string fileName, DateTime creationDate, DateTime updateDate);
    }
}
