using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarquitoUtils.Main.Sql.Entities
{
    /// <summary>
    /// An object represent a required property, needed for retrieve an entity from database
    /// </summary>
    public class PropertyConstraint<TEntity>
        where TEntity : Entity
    {
        /// <summary>
        /// The property's name
        /// </summary>
        public string PropertyName { get; private set; }
        /// <summary>
        /// The value of the property
        /// </summary>
        public object Value { get; private set; }
        /// <summary>
        /// The property is case sensitive ?
        /// </summary>
        public bool CaseSensitive { get; private set; }
        /// <summary>
        /// The class own this property
        /// </summary>
        public Type OwnerType { get; internal set; } = typeof(TEntity);
        /// <summary>
        /// The property name in parent class own this property
        /// </summary>
        public string ParentPropertyName { get; private set; }

        /// <summary>
        /// An object represent a required property, needed for retrieve an entity from database
        /// </summary>
        /// <param name="propertyName">The property's name</param>
        /// <param name="value">The value of the property</param>
        /// <param name="parentPropertyName">The property name in parent class own this property</param>
        /// <param name="caseSensitive">The property is case sensitive ?</param>
        public PropertyConstraint(string propertyName, object value, string parentPropertyName = "", bool caseSensitive = true)
        {
            this.PropertyName = propertyName;
            this.Value = value;
            this.ParentPropertyName = parentPropertyName;
            this.CaseSensitive = caseSensitive;
        }

        /// <summary>
        /// An object represent a required property, needed for retrieve an entity from database
        /// </summary>
        /// <param name="propertyName">The property's name</param>
        /// <param name="entity">An entity</param>
        /// <param name="parentPropertyName">The property name in parent class own this property</param>
        /// <param name="caseSensitive">The property is case sensitive ?</param>
        public PropertyConstraint(string propertyName, TEntity entity, string parentPropertyName = "", bool caseSensitive = true)
            : this(propertyName, entity.GetFieldValue<object>(propertyName), parentPropertyName, caseSensitive)
        {
            this.OwnerType = entity.GetType();
        }
    }
}
