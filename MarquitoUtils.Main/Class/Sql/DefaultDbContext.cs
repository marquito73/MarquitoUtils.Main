using MarquitoUtils.Main.Class.Entities.File;
using MarquitoUtils.Main.Class.Entities.Sql;
using MarquitoUtils.Main.Class.Entities.Sql.Translations;
using MarquitoUtils.Main.Class.Entities.Sql.UserTracking;
using MarquitoUtils.Main.Class.Tools;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

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
        /// Use cache for get data ?
        /// </summary>
        public bool UseCache { get; set; } = false;
        /// <summary>
        /// Database set of script histories
        /// </summary>
        public DbSet<ScriptHistory> ScriptHistory { get; set; }
        public DbSet<Translation> Translations { get; set; }
        public DbSet<TranslationField> TranslationsFields { get; set; }
        public DbSet<UserTrackHistory> UserTrackHistory { get; set; }

        /// <summary>
        /// Main constructor of default database context
        /// </summary>
        /// <param name="contextBuilder">Database context builder</param>
        /// <param name="dbConfig">Database configuration for connection</param>
        protected DefaultDbContext(DbContextOptionsBuilder contextBuilder, DatabaseConfiguration dbConfig)
            : base(contextBuilder.Options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Translation>()
                .HasOne(t => t.TranslationField)
                .WithMany()
                .HasForeignKey(t => t.TranslationFieldId);
            base.OnModelCreating(modelBuilder);
        }

        private IEnumerable<PropertyInfo> GetSubEntityProps(Type type)
        {
            return type.GetProperties()
                .Where(prop => prop.PropertyType.IsAnEntityType() || prop.PropertyType.IsGenericDbCollectionType());
        }

        protected void MapNavigation<TEntity>(ModelBuilder modelBuilder, params string[] excludedProperties)
            where TEntity : Entity
        {
            this.GetSubEntityProps(typeof(TEntity)).ForEach(prop =>
            {
                if (!Utils.Nvl(excludedProperties).Contains(prop.Name))
                {
                    modelBuilder.Entity<TEntity>().Navigation(prop.Name).AutoInclude();
                }
            });
        }

        /// <summary>
        /// Get database context
        /// </summary>
        /// <typeparam name="DB">The database context type</typeparam>
        /// <param name="dbConfig">Sql connection builder</param>
        /// <returns>The database context</returns>
        public static DB GetDbContext<DB>(DatabaseConfiguration dbConfig)
            where DB : DefaultDbContext
        {
            DbContextOptionsBuilder contextBuilder = new DbContextOptionsBuilder<DB>();
            contextBuilder.UseSqlServer(dbConfig.GetConnectionString());

            return (DB)Activator.CreateInstance(typeof(DB), contextBuilder, dbConfig);
        }
    }
}
