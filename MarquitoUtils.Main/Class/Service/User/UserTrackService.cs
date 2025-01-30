using MarquitoUtils.Main.Class.Entities.File;
using MarquitoUtils.Main.Class.Entities.Sql.Lists;
using MarquitoUtils.Main.Class.Entities.Sql.UserTracking;
using MarquitoUtils.Main.Class.Service.General;
using MarquitoUtils.Main.Class.Service.Sql;
using MarquitoUtils.Main.Class.Tools;

namespace MarquitoUtils.Main.Class.Service.User
{
    public class UserTrackService : DefaultService, IUserTrackService
    {
        public IEntityService EntityService { get; set; }

        public UserTrackHistory GetUserTrack(string IPAdress)
        {
            ListUserTrackHistory list = new ListUserTrackHistory(this.EntityService.DbContext);

            list.SetUserIPAddress(IPAdress);

            UserTrackHistory? userTrackHistory = list.GetEntityList().FirstOrDefault();

            if (Utils.IsNull(userTrackHistory))
            {
                userTrackHistory = new UserTrackHistory()
                {
                    UserTrackLastVisit = DateTime.Now,
                    VisitCount = 0,
                    IPAddress = IPAdress,
                };

                this.EntityService.PersistEntity(userTrackHistory);
            }

            return userTrackHistory;
        }

        public void SaveUserTrack(string IPAdress)
        {
            UserTrackHistory userTrackHistory = this.GetUserTrack(IPAdress);

            userTrackHistory.UserTrackLastVisit = DateTime.Now;
            userTrackHistory.VisitCount++;

            this.EntityService.FlushData(out Exception exception);
        }
    }
}
