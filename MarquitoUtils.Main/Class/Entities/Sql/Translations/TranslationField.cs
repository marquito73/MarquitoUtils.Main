﻿using System.ComponentModel.DataAnnotations;
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
        [Key, Column("translation_field_id", TypeName = "bigint"), Required]
        [Index("pk_translation_field", IsUnique = true)]
        public override int Id { get; set; }
        /// <summary>
        /// Id of entity need this translation field
        /// </summary>
        [Column("translation_entity_id", TypeName = "bigint"), Required]
        [Index("ix_translation_field", 1, IsUnique = true)]
        public int TranslationEntityId { get; set; }
        /// <summary>
        /// The entity class
        /// </summary>
        [Column("translation_entity_class", TypeName = "nvarchar"), MaxLength(256), Required]
        [Index("ix_translation_field", 2, IsUnique = true)]
        public string TranslationEntityClass { get; set; }
        /// <summary>
        /// The entity property
        /// </summary>
        [Column("translation_entity_property", TypeName = "nvarchar"), MaxLength(128), Required]
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

        public string GetTranslation(enumLang language)
        {
            return this.Translations.Where(translation => translation.Language.Equals(language))
                .Select(translation => translation.TranslationContent).FirstOrDefault();
        }
    }
}