using MarquitoUtils.Main.Class.Entities.Sql.UserTracking;
using MarquitoUtils.Main.Class.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarquitoUtils.Main.Class.Entities.Sql.Lists
{
    /// <summary>
    /// List of user track
    /// </summary>
    public class ListUserTrackHistory : EntityList<UserTrackHistory>
    {
        /// <summary>
        /// List of user track
        /// </summary>
        /// <param name="dbContext">The database context</param>
        public ListUserTrackHistory(DefaultDbContext dbContext) : base(dbContext)
        {
        }

        public void SetUserIPAddress(string userIPAddress)
        {
            this.AddEqualFilter(nameof(UserTrackHistory.IPAddress), userIPAddress);
        }
    }
}
