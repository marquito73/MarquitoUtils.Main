using MarquitoUtils.Main.Class.Attributes.Sql;
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
        [Required]
        [Key]
        [GenericColumn<int>("translation_id", isKey: true)]
        [Index("pk_translation", IsUnique = true)]
        public override int Id { get; set; }
        /// <summary>
        /// The translation language
        /// </summary>
        [Required]
        [GenericColumn<LanguageType>("language")]
        [Index("ix_translation", 1, IsUnique = true)]
        public LanguageType Language { get; set; }
        /// <summary>
        /// Id of translation field
        /// </summary>
        [Required]
        [GenericColumn<int>("translation_field_id", isKey: true)]
        [Index("ix_translation", 2, IsUnique = true)]
        [ForeignKey(nameof(TranslationField))]
        public int TranslationFieldId { get; set; } = -1;
        /// <summary>
        /// Translation field
        /// </summary>
        public virtual TranslationField? TranslationField { get; set; }
        /// <summary>
        /// The entity property
        /// </summary>
        [Required]
        [MaxLength(2000)]
        [GenericColumn<string>("translation_content")]
        [Index("ix_translation", 3, IsUnique = true)]
        public string TranslationContent { get; set; }

        public Translation()
        {

        }
    }
}
