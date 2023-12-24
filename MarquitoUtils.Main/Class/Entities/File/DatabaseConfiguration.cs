using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarquitoUtils.Main.Class.Entities.File
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
        /// The server source
        /// </summary>
        public string Source { get; }
        /// <summary>
        /// The database's name
        /// </summary>
        public string DatabaseName { get; }

        /// <summary>
        /// Database's configuration
        /// </summary>
        /// <param name="user">The user</param>
        /// <param name="password">The password</param>
        /// <param name="source">The server source</param>
        /// <param name="databaseName">The database's name</param>
        public DatabaseConfiguration(string user, string password, string source, string databaseName)
        {
            this.User = user;
            this.Password = password;
            this.Source = source;
            this.DatabaseName = databaseName;
        }
    }
}
