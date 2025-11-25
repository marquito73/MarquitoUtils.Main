using MarquitoUtils.Main.Sql.Entities.History;
using MarquitoUtils.Main.Sql.List;
using MarquitoUtils.Main.Sql.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarquitoUtils.Main.Sql.Entities.Lists
{
    /// <summary>
    /// List of scripts already executed on database
    /// </summary>
    public class ListScriptHistory : EntityList<ScriptHistory>
    {
        /// <summary>
        /// List of scripts already executed on database
        /// </summary>
        /// <param name="entityService">The entity service</param>
        public ListScriptHistory(IEntityService entityService) : base(entityService)
        {
        }
    }
}
