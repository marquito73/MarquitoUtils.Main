using MarquitoUtils.Main.Class.Attributes.Sql;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static MarquitoUtils.Main.Class.Enums.EnumLang;

namespace MarquitoUtils.Main.Class.Entities.Sql.Translations
{
    /// <summary>
    /// Translation field entity, the link between translatable entity property and translation
    /// (A translation field = an entity property translation)
    /// </summary>
    [Serializable]
    [Table("translation_field")]
    public class TranslationField : Entity
    {
        /// <summary>
        /// Id of the translation field
        /// </summary>
        [Required]
        [Key]
        [GenericColumn<int>("translation_field_id", isKey: true)]
        [Index("pk_translation_field", IsUnique = true)]
        public override int Id { get; set; }
        /// <summary>
        /// Id of entity need this translation field
        /// </summary>
        [Required]
        [GenericColumn<int>("translation_entity_id", isKey: true)]
        [Index("ix_translation_field", 1, IsUnique = true)]
        public int TranslationEntityId { get; set; }
        /// <summary>
        /// The entity class
        /// </summary>
        [Required]
        [MaxLength(256)]
        [GenericColumn<string>("translation_entity_class")]
        [Index("ix_translation_field", 2, IsUnique = true)]
        public string TranslationEntityClass { get; set; }
        /// <summary>
        /// The entity property
        /// </summary>
        [Required]
        [MaxLength(128)]
        [GenericColumn<string>("translation_entity_property")]
        [Index("ix_translation_field", 3, IsUnique = true)]
        public string TranslationEntityProperty { get; set; }

        /// <summary>
        /// Translations for this translation field
        /// </summary>
        [ForeignKey(nameof(Translation.TranslationFieldId))]
        public virtual ICollection<Translation>? Translations { get; set; } = new List<Translation>();

        public TranslationField()
        {

        }

        public string GetTranslation(LanguageType language)
        {
            return this.Translations.Where(translation => translation.Language.Equals(language))
                .Select(translation => translation.TranslationContent).FirstOrDefault();
        }

        public void SetTranslation(LanguageType language, string translation)
        {
            this.Translations.Where(translation => translation.Language.Equals(language))
                .First().TranslationContent = translation;
        }
    }
}
