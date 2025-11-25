
using MarquitoUtils.Main.Common.Services;
using MarquitoUtils.Main.Common.Tools;
using MarquitoUtils.Main.Sql.Entities.Lists;
using MarquitoUtils.Main.Sql.Entities.UserTracking;

namespace MarquitoUtils.Main.Sql.Services.User
{
    public class UserTrackService : DefaultService, IUserTrackService
    {
        public IEntityService EntityService { get; set; }

        public UserTrackHistory GetUserTrack(string IPAdress)
        {
            ListUserTrackHistory list = new ListUserTrackHistory(this.EntityService);

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
