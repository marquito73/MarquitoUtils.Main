using MarquitoUtils.Main.Class.Service.Files;
using MarquitoUtils.Main.Class.Service.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.Data.SqlClient;
using MarquitoUtils.Main.Class.Logger;
using MarquitoUtils.Main.Class.Entities.Sql;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MarquitoUtils.Main.Class.Tools;
using MarquitoUtils.Main.Class.Entities.Sql.Lists;
using MarquitoUtils.Main.Class.Entities.File;

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

        // TODO Voir pour avoir des entités (QueryResult) qu'on construit grâce au DataReader et qu'on renvoie
        /*public SqlDataReader GetQueryResult(string sqlQueryContent)
        {

            SqlDataReader reader = null;

            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                connection.Open();

                SqlCommand sqlQuery = connection.CreateCommand();

                // Prepare the query
                sqlQuery.Connection = connection;

                try
                {
                    sqlQuery.CommandText = sqlQueryContent;
                    // Execute the script content
                    sqlQuery.ExecuteNonQuery();

                    reader = sqlQuery.ExecuteReader();

                    reader.
                }
                catch (Exception ex)
                {
                    Logger.Logger.Error("An error occurs with the query");
                }
            }

            return reader;
        }*/

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

        /// <summary>
        /// Add script name inside script history table
        /// </summary>
        /// <param name="scriptName"></param>
        private void AddNewScriptHistory(string scriptName)
        {
            if (!scriptName.Equals("001_ScriptHistory", StringComparison.OrdinalIgnoreCase))
            {
                ScriptHistory history = new ScriptHistory(scriptName, DateTime.UtcNow);
                this.EntityService.PersistEntity(history);
            }
        }
    }
}
