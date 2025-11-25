using MarquitoUtils.Main.Sql.Attributes;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarquitoUtils.Main.Sql.Entities.History
{
    /// <summary>
    /// Script history entity, contain sql scripts already executed
    /// </summary>
    [Serializable]
    [Table("script_history")]
    [Index(nameof(Id), IsUnique = true, Name = "pk_script_history")]
    public class ScriptHistory : Entity
    {
        /// <summary>
        /// Id of the script history
        /// </summary>
        [Required]
        [Key]
        [GenericColumn<int>("script_history_id", isKey: true)]
        public override int Id { get; set; }
        /// <summary>
        /// Script's name
        /// </summary>
        [Required]
        [MaxLength(250)]
        [GenericColumn<string>("script_name")]
        public string ScriptName { get; set; }
        /// <summary>
        /// Script's datetime execution
        /// </summary>
        [Required]
        [GenericColumn<DateTime>("dt_script_dt")]
        public DateTime DtScriptDt { get; set; } = DateTime.Now;

        /// <summary>
        /// Script history entity
        /// </summary>
        public ScriptHistory()
        {

        }

        /// <summary>
        /// Script history entity
        /// </summary>
        /// <param name="scriptName">Script name</param>
        /// <param name="dtScriptDt">Script's datetime execution</param>
        public ScriptHistory(string scriptName, DateTime dtScriptDt) : this()
        {
            ScriptName = scriptName;
            DtScriptDt = dtScriptDt;
        }
    }
}
