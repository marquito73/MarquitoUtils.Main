using MarquitoUtils.Main.Common.Tools;
using System.Text;

namespace MarquitoUtils.Main.Sql.Entities
{
    /// <summary>
    /// Database's configuration
    /// </summary>
    public class DatabaseConfiguration
    {
        /// <summary>
        /// The user
        /// </summary>
        public string User { get; }
        /// <summary>
        /// The password
        /// </summary>
        public string Password { get; }
        /// <summary>
        /// The server name
        /// </summary>
        public string ServerName { get; }
        /// <summary>
        /// The server's instance name
        /// </summary>
        public string InstanceName { get; }
        /// <summary>
        /// The database's name
        /// </summary>
        public string DatabaseName { get; }
        /// <summary>
        /// A connection string, if we need a custom connection differ with this object params
        /// </summary>
        public string ConnectionString { get; }

        /// <summary>
        /// Database's configuration
        /// </summary>
        /// <param name="user">The user</param>
        /// <param name="password">The password</param>
        /// <param name="source">The server source</param>
        /// <param name="databaseName">The database's name</param>
        public DatabaseConfiguration(string user, string password, string serverName, string instanceName, string databaseName)
        {
            this.User = user;
            this.Password = password;
            this.ServerName = serverName;
            this.InstanceName = instanceName;
            this.DatabaseName = databaseName;
        }

        public DatabaseConfiguration(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        /// <summary>
        /// Get connection string for database
        /// </summary>
        /// <returns>Connection string for database</returns>
        public string GetConnectionString()
        {
            StringBuilder sbConnectionString = new StringBuilder();

            if (Utils.IsNotEmpty(this.ConnectionString))
            {
                sbConnectionString.Append(ConnectionString);
            }
            else
            {
                sbConnectionString.Append($"Server={this.ServerName}\\{this.InstanceName};")
                    .Append($" Database={this.DatabaseName};")
                    .Append($" User Id={this.User};")
                    .Append($" Password={this.Password};")
                    .Append(" Trusted_Connection=True;")
                    .Append(" Encrypt=False;")
                    .Append(" MultipleActiveResultSets=True;");
            }

            return sbConnectionString.ToString();
        }
    }
}
