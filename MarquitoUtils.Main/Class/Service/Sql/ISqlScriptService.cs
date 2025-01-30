using MarquitoUtils.Main.Class.Entities.File;
using MarquitoUtils.Main.Class.Service.Files;
using MarquitoUtils.Main.Class.Service.General;

namespace MarquitoUtils.Main.Class.Service.Sql
{
    /// <summary>
    /// Service for SQL
    /// </summary>
    public interface ISqlScriptService : DefaultService
    {
        /// <summary>
        /// The configuration for connect to the database
        /// </summary>
        public DatabaseConfiguration DatabaseConfiguration { get; set; }
        /// <summary>
        /// File service
        /// </summary>
        public IFileService FileService { get; set; }
        /// <summary>
        /// Entity service
        /// </summary>
        public IEntityService EntityService { get; set; }

        /// <summary>
        /// Check if table exist on database
        /// </summary>
        /// <param name="tableName">The table's name</param>
        /// <returns>Table exist on database ?</returns>
        public bool CheckIfTableExist(string tableName);

        /// <summary>
        /// Execute sql script
        /// </summary>
        /// <param name="scriptName">Sql script name</param>
        /// <param name="scriptContent">Sql script content</param>
        /// <param name="checkExistence">We need to check if this script has already been executed ?</param>
        public void ExecuteSqlScript(string scriptName, string scriptContent, bool checkExistence = true);
    }
}
