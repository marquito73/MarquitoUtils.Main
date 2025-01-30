using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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
        [Key, Column("script_history_id", TypeName = "bigint"), Required]
        [Index("PK_script_history", IsUnique = true)]
        public override int Id { get; set; }
        /// <summary>
        /// Script's name
        /// </summary>
        [Column("script_name", TypeName = "nvarchar"), Required]
        public string ScriptName { get; set; }
        /// <summary>
        /// Script's datetime execution
        /// </summary>
        [Column("dt_script_dt", TypeName = "datetime"), Required]
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
