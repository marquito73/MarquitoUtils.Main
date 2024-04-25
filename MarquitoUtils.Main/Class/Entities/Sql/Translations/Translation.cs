using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static MarquitoUtils.Main.Class.Enums.EnumLang;

namespace MarquitoUtils.Main.Class.Entities.Sql.Translations
{
    /// <summary>
    /// Translation entity, contain entities's translations
    /// </summary>
    [Serializable]
    [Table("translation")]
    public class Translation : Entity
    {
        /// <summary>
        /// Id of the translation
        /// </summary>
        [Key, Column("translation_id", TypeName = "bigint"), Required]
        [Index("pk_translation", IsUnique = true)]
        public override int Id { get; set; }
        /// <summary>
        /// The translation language
        /// </summary>
        [Column("language", TypeName = "tinyint"), Required]
        [Index("ix_translation", 1, IsUnique = true)]
        public enumLang Language { get; set; }
        /// <summary>
        /// Id of translation field
        /// </summary>
        [ForeignKey(nameof(TranslationField))]
        [Column("translation_field_id", TypeName = "bigint"), Required]
        [Index("ix_translation", 2, IsUnique = true)]
        public int TranslationFieldId { get; set; } = -1;
        /// <summary>
        /// Translation field
        /// </summary>
        public virtual TranslationField? TranslationField { get; set; }
        /// <summary>
        /// The entity property
        /// </summary>
        [Column("translation_content", TypeName = "nvarchar"), MaxLength(2000), Required]
        [Index("ix_translation", 3, IsUnique = true)]
        public string TranslationContent { get; set; }

        public Translation()
        {

        }
    }
}
