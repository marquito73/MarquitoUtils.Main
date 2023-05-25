using MarquitoUtils.Main.Class.Entities.Sql;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarquitoUtils.Main.Class.Sql
{
    /// <summary>
    /// Default database context, contains DbSets for entities
    /// </summary>
    public abstract class DefaultDbContext : DbContext
    {
        /// <summary>
        /// Sql connection string
        /// </summary>
        private string SqlConnectionString { get; set; }
        /// <summary>
        /// Database set of script histories
        /// </summary>
        public DbSet<ScriptHistory> ScriptHistories { get; set; }

        /// <summary>
        /// Main constructor of default database context
        /// </summary>
        /// <param name="contextBuilder">Database context builder</param>
        /// <param name="sqlBuilder">Sql connection builder</param>
        protected DefaultDbContext(DbContextOptionsBuilder contextBuilder, SqlConnectionBuilder sqlBuilder) 
            : base(contextBuilder.Options)
        {
            this.SqlConnectionString = sqlBuilder.GetConnectionString();

            contextBuilder
                .UseLazyLoadingProxies(true)
                .UseSqlServer(this.SqlConnectionString);
        }

        /// <summary>
        /// Get database context
        /// </summary>
        /// <typeparam name="DB">The database context type</typeparam>
        /// <param name="sqlBuilder">Sql connection builder</param>
        /// <returns>The database context</returns>
        public static DB GetDbContext<DB>(SqlConnectionBuilder sqlBuilder) where DB : DefaultDbContext
        {
            DbContextOptionsBuilder contextBuilder = new DbContextOptionsBuilder<DB>();
            contextBuilder.UseSqlServer(sqlBuilder.GetConnectionString());

            return (DB)Activator.CreateInstance(typeof(DB), contextBuilder, sqlBuilder);
        }
    }
}
