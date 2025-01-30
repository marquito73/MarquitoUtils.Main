using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarquitoUtils.Main.Class.Entities.Sql.UserTracking
{
    /// <summary>
    /// User track history entity, contain user track data
    /// </summary>
    [Serializable]
    [Table("user_track_history")]
    public class UserTrackHistory : Entity
    {
        /// <summary>
        /// Id of the user track history
        /// </summary>
        [Key, Column("user_track_history_id", TypeName = "bigint"), Required]
        [Index("PK_user_track_history", IsUnique = true)]
        public override int Id { get; set; }
        /// <summary>
        /// User track datetime execution
        /// </summary>
        [Column("dt_user_track_last_visit_dt", TypeName = "datetime"), Required]
        public DateTime UserTrackLastVisit { get; set; } = DateTime.Now;
        /// <summary>
        /// Script's datetime execution
        /// </summary>
        [Column("visit_count", TypeName = "int"), Required]
        public int VisitCount { get; set; }
        /// <summary>
        /// IP adress
        /// </summary>
        [Column("ip_address", TypeName = "nvarchar"), MaxLength(20), Required]
        public string IPAddress { get; set; } = "";
    }
}
