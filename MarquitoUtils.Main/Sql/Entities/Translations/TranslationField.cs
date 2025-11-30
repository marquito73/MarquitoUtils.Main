using MarquitoUtils.Main.Sql.Attributes;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static MarquitoUtils.Main.Translate.Enums.Language.EnumLang;

namespace MarquitoUtils.Main.Sql.Entities.Translations
{
    /// <summary>
    /// Translation field entity, the link between translatable entity property and translation
    /// (A translation field = an entity property translation)
    /// </summary>
    [Serializable]
    [Table("translation_field")]
    [Index(nameof(Id), IsUnique = true, Name = "pk_translation_field")]
    [Index(nameof(TranslationEntityId), nameof(TranslationEntityClass), nameof(TranslationEntityProperty), IsUnique = true, Name = "ix_translation_field")]
    public class TranslationField : Entity
    {
        /// <summary>
        /// Id of the translation field
        /// </summary>
        [Required]
        [Key]
        [GenericColumn<int>("translation_field_id", isKey: true)]
        public override int Id { get; set; }
        /// <summary>
        /// Id of entity need this translation field
        /// </summary>
        [Required]
        [GenericColumn<int>("translation_entity_id", isKey: true)]
        public int TranslationEntityId { get; set; }
        /// <summary>
        /// The entity class
        /// </summary>
        [Required]
        [MaxLength(256)]
        [GenericColumn<string>("translation_entity_class")]
        public string TranslationEntityClass { get; set; }
        /// <summary>
        /// The entity property
        /// </summary>
        [Required]
        [MaxLength(128)]
        [GenericColumn<string>("translation_entity_property")]
        public string TranslationEntityProperty { get; set; }

        /// <summary>
        /// Translations for this translation field
        /// </summary>
        [ForeignKey(nameof(Translation.TranslationFieldId))]
        public virtual ICollection<Translation>? Translations { get; set; } = new List<Translation>();

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
