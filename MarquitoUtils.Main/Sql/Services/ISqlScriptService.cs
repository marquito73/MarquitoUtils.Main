using MarquitoUtils.Main.Common.Services;
using MarquitoUtils.Main.Files.Services;
using MarquitoUtils.Main.Sql.Entities;

namespace MarquitoUtils.Main.Sql.Services
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
        /// <typeparam name="TEntity">Entity</typeparam>
        /// <returns>Table exist on database ?</returns>
        public bool CheckIfTableExist<TEntity>()
            where TEntity : Entity;

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

        /// <summary>
        /// Execute entity sql script
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="checkExistence">We need to check if this script has already been executed ?</param>
        public void ExecuteEntitySqlScript<TEntity>(bool checkExistence = true)
            where TEntity : Entity;

        /// <summary>
        /// Execute entity sql script
        /// </summary>
        /// <param name="entityType">Entity type</param>
        /// <param name="checkExistence">We need to check if this script has already been executed ?</param>
        public void ExecuteEntitySqlScript(Type entityType, bool checkExistence = true);
    }
}
