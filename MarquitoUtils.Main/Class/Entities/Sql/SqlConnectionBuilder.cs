using System.Data.SqlClient;
using System.Text;

namespace MarquitoUtils.Main.Class.Entities.Sql
{
    /// <summary>
    /// Class for build DB connection string
    /// </summary>
    public class SqlConnectionBuilder
    {
        /// <summary>
        /// Database's user
        /// </summary>
        public string User { get; private set; } = "";
        /// <summary>
        /// Database's user password
        /// </summary>
        public string Password { get; private set; } = "";
        /// <summary>
        /// Database's source
        /// </summary>
        public string Source { get; private set; } = "";
        /// <summary>
        /// Database's name
        /// </summary>
        public string DataBase { get; private set; } = "";
        public SqlConnectionBuilder(string user, string password, string source, string dataBase = "")
        {
            this.User = user;
            this.Password = password;
            this.Source = source;
            this.DataBase = dataBase;
        }

        /// <summary>
        /// Get connection string for database
        /// </summary>
        /// <returns>Connection string for database</returns>
        public string GetConnectionString()
        {
            StringBuilder sbConnectionString = new StringBuilder();

            sbConnectionString.Append(@"Data source=").Append(this.Source)
                .Append("; Initial Catalog=").Append(this.DataBase)
                .Append("; User=").Append(this.User)
                .Append("; Password=").Append(this.Password)
                .Append("; TrustServerCertificate=True")
                .Append("; Encrypt=False");

            // Work too
            // Server=this.Source;Database=this.Catalog;User=this.User;Password=this.Password;TrustServerCertificate=True;

            return sbConnectionString.ToString();
        }

        /// <summary>
        /// Get sql server connection
        /// </summary>
        /// <returns>Sql server connection</returns>
        public SqlConnection GetSqlServerConnection()
        {
            return new SqlConnection(this.GetConnectionString());
        }
    }
}
