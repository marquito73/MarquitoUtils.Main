using MarquitoUtils.Main.Class.Attributes.Sql;
using Microsoft.EntityFrameworkCore;
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
    [Index(nameof(Id), IsUnique = true, Name = "pk_translation")]
    [Index(nameof(Language), nameof(TranslationFieldId), nameof(TranslationContent), IsUnique = true, Name = "ix_translation")]
    public class Translation : Entity
    {
        /// <summary>
        /// Id of the translation
        /// </summary>
        [Required]
        [Key]
        [GenericColumn<int>("translation_id", isKey: true)]
        public override int Id { get; set; }
        /// <summary>
        /// The translation language
        /// </summary>
        [Required]
        [GenericColumn<LanguageType>("language")]
        public LanguageType Language { get; set; }
        /// <summary>
        /// Id of translation field
        /// </summary>
        [Required]
        [GenericColumn<int>("translation_field_id", isKey: true)]
        //[Column("translation_field_id")]
        [ForeignKey(nameof(TranslationField))]
        public int TranslationFieldId { get; set; } = -1;
        /// <summary>
        /// Translation field
        /// </summary>
        //[ForeignKey(nameof(TranslationFieldId))]
        public virtual TranslationField? TranslationField { get; set; }
        /// <summary>
        /// The entity property
        /// </summary>
        [Required]
        [MaxLength(2000)]
        [GenericColumn<string>("translation_content")]
        public string TranslationContent { get; set; }

        public Translation()
        {

        }
    }
}
