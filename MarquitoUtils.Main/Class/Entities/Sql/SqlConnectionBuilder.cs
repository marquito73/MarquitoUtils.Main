using System.Data.SqlClient;
using System.Text;

namespace MarquitoUtils.Main.Class.Entities.Sql
{
    public class SqlConnectionBuilder
    {
        public string User { get; private set; } = "";
        public string Password { get; private set; } = "";
        public string Source { get; private set; } = "";
        public string Catalog { get; private set; } = "";
        public SqlConnectionBuilder(string user, string password, string source, string catalog = "")
        {
            this.User = user;
            this.Password = password;
            this.Source = source;
            this.Catalog = catalog;
        }

        public string GetConnectionString()
        {
            StringBuilder sbConnectionString = new StringBuilder();

            sbConnectionString.Append(@"Data source=").Append(this.Source)
                .Append(";Initial Catalog=").Append(this.Catalog)
                .Append("; User=").Append(this.User)
                .Append("; Password=").Append(this.Password);

            return sbConnectionString.ToString();
        }

        public SqlConnection GetSqlServerConnection()
        {
            return new SqlConnection(this.GetConnectionString());
        }

        /*public WebAppTestContext GetDbContext()
        {
            DbContextOptionsBuilder contextBuilder = new DbContextOptionsBuilder<WebAppTestContext>();
            contextBuilder.UseSqlServer(this.GetConnectionString());

            return new WebAppTestContext(contextBuilder);
        }*/

    }
}
