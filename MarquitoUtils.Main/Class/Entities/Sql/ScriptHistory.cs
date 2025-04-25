using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MarquitoUtils.Main.Class.Attributes.Sql;

namespace MarquitoUtils.Main.Class.Entities.Sql
{
    /// <summary>
    /// Script history entity, contain sql scripts already executed
    /// </summary>
    [Serializable]
    [Table("script_history")]
    public class ScriptHistory : Entity
    {
        /// <summary>
        /// Id of the script history
        /// </summary>
        [Required]
        [Key]
        [GenericColumn<int>("script_history_id", isKey: true)]
        [Index("pk_script_history", IsUnique = true)]
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
