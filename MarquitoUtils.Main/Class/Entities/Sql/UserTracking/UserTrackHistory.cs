using MarquitoUtils.Main.Class.Attributes.Sql;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarquitoUtils.Main.Class.Entities.Sql.UserTracking
{
    /// <summary>
    /// User track history entity, contain user track data
    /// </summary>
    [Serializable]
    [Table("user_track_history")]
    [Index(nameof(Id), IsUnique = true, Name = "pk_user_track_history")]
    public class UserTrackHistory : Entity
    {
        /// <summary>
        /// Id of the user track history
        /// </summary>
        [Required]
        [Key]
        [GenericColumn<int>("user_track_history_id", isKey: true)]
        public override int Id { get; set; }
        /// <summary>
        /// User track datetime execution
        /// </summary>
        [Required]
        [GenericColumn<DateTime>("dt_user_track_last_visit_dt")]
        public DateTime UserTrackLastVisit { get; set; } = DateTime.Now;
        /// <summary>
        /// Script's datetime execution
        /// </summary>
        [Required]
        [GenericColumn<int>("visit_count")]
        public int VisitCount { get; set; }
        /// <summary>
        /// IP adress
        /// </summary>
        [Required]
        [MaxLength(20)]
        [GenericColumn<string>("ip_address")]
        public string IPAddress { get; set; } = "";
    }
}
