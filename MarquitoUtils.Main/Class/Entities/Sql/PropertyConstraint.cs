using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarquitoUtils.Main.Class.Entities.Sql
{
    /// <summary>
    /// An object represent a required property, needed for retrieve an entity from database
    /// </summary>
    public class PropertyConstraint
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
        public Type OwnerType { get; private set; }
        /// <summary>
        /// The property name in parent class own this property
        /// </summary>
        public string ParentPropertyName { get; private set; }

        /// <summary>
        /// An object represent a required property, needed for retrieve an entity from database
        /// </summary>
        /// <param name="propertyName">The property's name</param>
        /// <param name="value">The value of the property</param>
        /// <param name="entityType">The class own this property</param>
        /// <param name="parentPropertyName">The property name in parent class own this property</param>
        /// <param name="caseSensitive">The property is case sensitive ?</param>
        public PropertyConstraint(string propertyName, object value, Type entityType, 
            string parentPropertyName = "", bool caseSensitive = true)
        {
            this.PropertyName = propertyName;
            this.Value = value;
            this.OwnerType = entityType;
            this.ParentPropertyName = parentPropertyName;
            this.CaseSensitive = caseSensitive;
        }

        /// <summary>
        /// An object represent a required property, needed for retrieve an entity from database
        /// </summary>
        /// <param name="entity">An entity</param>
        /// <param name="propertyName">The property's name</param>
        /// <param name="parentPropertyName">The property name in parent class own this property</param>
        /// <param name="caseSensitive">The property is case sensitive ?</param>
        public PropertyConstraint(Entity entity, string propertyName, string parentPropertyName = "", bool caseSensitive = true)
        {
            this.PropertyName = propertyName;
            this.Value = entity.GetFieldValue<object>(propertyName);
            this.OwnerType = entity.GetType();
            this.ParentPropertyName = parentPropertyName;
            this.CaseSensitive = caseSensitive;
        }
    }
}
