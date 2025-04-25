using MarquitoUtils.Main.Class.Service.Files;
using MarquitoUtils.Main.Class.Service.General;
using System.Text;
using Microsoft.Data.SqlClient;
using MarquitoUtils.Main.Class.Entities.Sql;
using MarquitoUtils.Main.Class.Tools;
using MarquitoUtils.Main.Class.Entities.Sql.Lists;
using MarquitoUtils.Main.Class.Entities.File;
using MarquitoUtils.Main.Class.Tools.Sql;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarquitoUtils.Main.Class.Service.Sql
{
    /// <summary>
    /// Service for SQL
    /// </summary>
    public class SqlScriptService : DefaultService, ISqlScriptService
    {
        public DatabaseConfiguration DatabaseConfiguration { get; set; }
        public IFileService FileService { get; set; }
        public IEntityService EntityService { get; set; }

        public SqlScriptService(DatabaseConfiguration dbConfig)
        {
            this.DatabaseConfiguration = dbConfig;
            this.FileService = new FileService();
        }

        private string GetConnectionString()
        {
            return this.DatabaseConfiguration.GetConnectionString();
        }

        public bool CheckIfTableExist<TEntity>()
            where TEntity : Entity
        {
            return this.CheckIfTableExist(typeof(TEntity).GetCustomAttribute<TableAttribute>().Name);
        }

        public bool CheckIfTableExist(string tableName)
        {
            bool tableExist = false;

            StringBuilder selectQuery = new StringBuilder()
                    .Append("select * from information_schema.tables where table_name = '")
                    .Append(tableName).Append("'");

            using (SqlConnection connection = new SqlConnection(this.GetConnectionString()))
            {
                connection.Open();

                SqlCommand sqlQuery = connection.CreateCommand();

                // Prepare the query
                sqlQuery.Connection = connection;

                try
                {
                    sqlQuery.CommandText = selectQuery.ToString();
                    // Execute the script content
                    sqlQuery.ExecuteNonQuery();

                    SqlDataReader reader = sqlQuery.ExecuteReader();

                    tableExist = reader.HasRows;
                }
                catch (Exception ex)
                {
                    Logger.Logger.Error("An error occurs when try to check if table exist: " + tableName);
                }
            }

            return tableExist;
        }

        public void ExecuteSqlScript(string scriptName, string scriptContent, bool checkExistence = true)
        {
            bool scriptAlreadyExecuted = false;

            if (checkExistence)
            {
                ListScriptHistory scriptHistories = new ListScriptHistory(this.EntityService.DbContext);

                scriptAlreadyExecuted = scriptHistories.GetEntityList().Any(script => script.ScriptName.Equals(scriptName));
            }

            if (!scriptAlreadyExecuted)
            {
                using (SqlConnection connection = new SqlConnection(this.GetConnectionString()))
                {
                    connection.Open();

                    SqlCommand sqlQuery = connection.CreateCommand();

                    SqlTransaction sqlTransaction;
                    // Start a local transaction.
                    sqlTransaction = connection.BeginTransaction();
                    // Prepare the query
                    sqlQuery.Connection = connection;
                    sqlQuery.Transaction = sqlTransaction;

                    try
                    {
                        foreach (string sqlScriptContent in Utils.Split(scriptContent, "\nGO"))
                        {
                            if (Utils.IsNotEmpty(sqlScriptContent.Trim()))
                            {
                                sqlQuery.CommandText = sqlScriptContent;
                                // Execute the script content inside the transaction
                                sqlQuery.ExecuteNonQuery();
                            }
                        }
                        // Try commit the transaction
                        sqlTransaction.Commit();

                        this.AddNewScriptHistory(scriptName);
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            // Error occurs, rollback transaction
                            sqlTransaction.Rollback();
                        }
                        catch (Exception ex2)
                        {
                            // Log if rollback doesn't work
                            Logger.Logger.Error("A transaction has rollback : " + ex2.GetType() + " : " + ex2.Message);
                        }
                    }
                }
            }
        }

        public void ExecuteEntitySqlScript<TEntity>(bool checkExistence = true) where TEntity : Entity
        {
            this.ExecuteSqlScript(typeof(TEntity).Name, new EntitySqlScriptHelperTest<TEntity>()
                .RenderEntitySqlScript(), checkExistence);
        }

        public void ExecuteEntitySqlScript(Type entityType, bool checkExistence = true)
        {
            // Get generics type parameters, and methods type parameters
            Type[] genericArgs = { entityType };
            Type[] methodsArgs = { typeof(bool) };
            // Then get the ExecuteEntitySqlScript method
            MethodInfo method = this.GetType().GetMethod(nameof(ExecuteEntitySqlScript), methodsArgs).MakeGenericMethod(genericArgs);
            // Finally, execute it
            method.Invoke(this, new object[] { checkExistence });
        }

        /// <summary>
        /// Add script name inside script history table
        /// </summary>
        /// <param name="scriptName"></param>
        private void AddNewScriptHistory(string scriptName)
        {
            ScriptHistory history = new ScriptHistory(scriptName, DateTime.UtcNow);
            this.EntityService.PersistEntity(history);
        }
    }
}
