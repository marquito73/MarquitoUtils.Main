using MarquitoUtils.Main.Class.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarquitoUtils.Main.Class.Entities.Sql.Lists
{
    /// <summary>
    /// List of scripts already executed on database
    /// </summary>
    public class ListScriptHistory : EntityList<ScriptHistory>
    {
        /// <summary>
        /// List of scripts already executed on database
        /// </summary>
        /// <param name="dbContext">The database context</param>
        public ListScriptHistory(DefaultDbContext dbContext) : base(dbContext)
        {
        }
    }
}
