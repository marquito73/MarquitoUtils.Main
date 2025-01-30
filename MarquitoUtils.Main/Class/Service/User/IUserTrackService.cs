using MarquitoUtils.Main.Class.Entities.File;
using MarquitoUtils.Main.Class.Entities.Sql.UserTracking;
using MarquitoUtils.Main.Class.Service.General;
using MarquitoUtils.Main.Class.Service.Sql;

namespace MarquitoUtils.Main.Class.Service.User
{
    public interface IUserTrackService : DefaultService
    {
        /// <summary>
        /// Entity service
        /// </summary>
        public IEntityService EntityService { get; set; }

        public UserTrackHistory GetUserTrack(string IPAdress);

        public void SaveUserTrack(string IPAdress);
    }
}
