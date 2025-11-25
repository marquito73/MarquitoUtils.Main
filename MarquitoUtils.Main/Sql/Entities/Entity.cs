using MarquitoUtils.Main.Common.Enums;
using MarquitoUtils.Main.Common.Tools;
using MarquitoUtils.Main.Sql.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace MarquitoUtils.Main.Sql.Entities
{
    [Serializable]
    public class Entity : IEntity, IDisposable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }
        /// <summary>
        /// The entity's identity code
        /// </summary>
        [NotMapped]
        public Guid EntityIdentityCode { get; private set; }

        public Entity()
        {
            this.EntityIdentityCode = Guid.NewGuid();
        }

        public PropertyInfo GetPropertyInfo(string fieldName)
        {
            return this.GetType().GetProperty(fieldName);
        }

        public Type GetFieldType(string fieldName)
        {
            return this.GetPropertyInfo(fieldName).PropertyType;
        }

        public T GetFieldValue<T>(string fieldName)
        {
            return (T)this.GetPropertyInfo(fieldName).GetValue(this, null);
        }

        public bool FieldEquals<TFieldType>(string fieldName, TFieldType value, bool TrimIfString = false)
        {
            bool fieldEquals;

            TFieldType fieldValue = this.GetFieldValue<TFieldType>(fieldName);

            if (Utils.IsNotNull(fieldValue))
            {
                if (typeof(TFieldType).Equals(typeof(string)))
                {
                    fieldEquals = (fieldValue as string).Trim().Equals((value as string).Trim());
                }
                else
                {
                    fieldEquals = fieldValue.Equals(value);
                }
            }
            else
            {
                fieldEquals = false;
            }

            return fieldEquals;
        }

        public void SetFieldValue(string fieldName, object value)
        {
            this.GetPropertyInfo(fieldName).SetValue(this, value, null);
        }

        public DataType GetDataType(string fieldName)
        {
            DataType dataType = DataType.Custom;

            DataTypeAttribute attribute = this.GetPropertyInfo(fieldName).GetCustomAttribute<DataTypeAttribute>();

            if (Utils.IsNotNull(attribute))
            {
                dataType = attribute.DataType;
            }

            return dataType;
        }

        public PropertyAttributes GetFieldAttributes(string fieldName)
        {
            return this.GetPropertyInfo(fieldName).Attributes;
        }

        public string GetTableName()
        {
            return Utils.GetAsString(this.GetType().GetCustomAttributesData()
                .Where(attr => attr.AttributeType.IsAssignableFrom(typeof(TableAttribute)))
                .ToList().First().ConstructorArguments.First().Value);
        }

        /// <summary>
        /// Get property's constraints of this entity
        /// </summary>
        /// <param name="getSubEntities">Get property's constraints of sub entities ?</param>
        /// <returns>Property's constraints of this entity</returns>
        public IEnumerable<PropertyConstraint<Entity>> GetPropertyConstraints(bool getSubEntities = true)
        {
            // Get required fields
            List<PropertyConstraint<Entity>> constraints = this.GetType().GetProperties()
                .Where(prop => !prop.Name.Equals(nameof(this.Id)))
                .Where(prop => prop.HasAttribute<ColumnAttribute>())
                .Where(prop => prop.HasAttribute<RequiredAttribute>())
                .Where(prop => !prop.HasAttribute<ForeignKeyAttribute>())
                .Where(prop => prop.HasIndexAttribute())
                .Select(prop => new PropertyConstraint<Entity>(prop.Name, this))
                .ToList();
            // Get required field of sub entities's required field
            if (getSubEntities)
            {
                this.GetType().GetProperties()
                    .Where(prop => !prop.Name.Equals(nameof(this.Id)))
                    .Where(prop => prop.HasAttribute<ColumnAttribute>())
                    .Where(prop => prop.HasAttribute<RequiredAttribute>() || prop.HasAttribute<DependencyColumnAttribute>())
                    .Where(prop => prop.HasAttribute<ForeignKeyAttribute>())
                    .Where(prop =>
                    {
                        bool dependentColumnIsValid = true;

                        if (prop.HasAttribute<DependencyColumnAttribute>())
                        {
                            DependencyColumnAttribute dependencyColumn = prop.GetCustomAttribute<DependencyColumnAttribute>();

                            dependentColumnIsValid = dependencyColumn.DependentValue
                                .Equals(this.GetFieldValue<object>(dependencyColumn.ColumnName));
                        }

                        return dependentColumnIsValid;
                    })
                    .Select(prop =>
                    {
                        List<PropertyConstraint<Entity>> subPropertyConstraints = new List<PropertyConstraint<Entity>>();

                        ForeignKeyAttribute foreignKey = prop.GetCustomAttribute<ForeignKeyAttribute>();

                        Entity subEntity = (Entity)this.GetFieldValue<object>(foreignKey.Name);

                        return subEntity.GetType().GetProperties()
                            .Where(subProp => !subProp.Name.Equals(nameof(this.Id)))
                            .Where(subProp => subProp.HasAttribute<ColumnAttribute>())
                            .Where(subProp => subProp.HasAttribute<RequiredAttribute>())
                            .Where(prop => prop.HasIndexAttribute())
                            .Where(subProp => !subProp.HasAttribute<ForeignKeyAttribute>())
                            .Select(subProp => new PropertyConstraint<Entity>(subProp.Name, subEntity, foreignKey.Name));
                    })
                    .SelectMany(prop => prop)
                    .ToList().ForEach(constraints.Add);
            }

            return constraints;
        }

        public EnumContentType GetContentType(string columnName)
        {
            EnumContentType contentType;

            object value = this.GetFieldValue<object>(columnName);

            switch (this.GetDataType(columnName))
            {
                case DataType.Password:
                    contentType = EnumContentType.Password;
                    break;
                case DataType.Currency:
                    contentType = EnumContentType.Currency;
                    break;
                case DataType.EmailAddress:
                    contentType = EnumContentType.EmailAddress;
                    break;
                case DataType.PhoneNumber:
                    contentType = EnumContentType.PhoneNumber;
                    break;
                case DataType.Upload:
                    contentType = EnumContentType.Binary;
                    break;
                case DataType.Custom:
                default:
                    switch (value)
                    {
                        case bool b:
                            contentType = EnumContentType.Boolean;
                            break;
                        case long l:
                            contentType = EnumContentType.Number;
                            break;
                        case int i:
                            contentType = EnumContentType.Number;
                            break;
                        case short s:
                            contentType = EnumContentType.Number;
                            break;
                        case string s:
                            contentType = EnumContentType.Text;
                            break;
                        default:
                            contentType = EnumContentType.Text;
                            break;
                    }
                    break;
            }

            return contentType;
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
