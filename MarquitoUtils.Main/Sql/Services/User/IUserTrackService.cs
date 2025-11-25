using MarquitoUtils.Main.Common.Services;
using MarquitoUtils.Main.Sql.Entities.UserTracking;

namespace MarquitoUtils.Main.Sql.Services.User
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
