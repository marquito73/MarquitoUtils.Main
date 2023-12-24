﻿using MarquitoUtils.Main.Class.Tools;
using MarquitoUtils.Main.Class.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using MarquitoUtils.Main.Class.Entities.Sql.Attributes;

namespace MarquitoUtils.Main.Class.Entities.Sql
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
            this.EntityIdentityCode = Utils.GetIdentityCode(this);
        }

        public PropertyInfo GetPropertyInfo(string fieldName)
        {
            return this.GetType().GetProperty(fieldName);
        }

        public Type GetFieldType(string fieldName)
        {
            return this.GetPropertyInfo(fieldName).PropertyType;
        }

        public object GetFieldValue(string fieldName)
        {
            return this.GetPropertyInfo(fieldName).GetValue(this, null);
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
        public IEnumerable<PropertyConstraint> GetPropertyConstraints(bool getSubEntities = true)
        {
            // Get required fields
            List<PropertyConstraint> constraints = this.GetType().GetProperties()
                .Where(prop => !prop.Name.Equals(nameof(this.Id)))
                .Where(prop => prop.HasAttribute<ColumnAttribute>())
                .Where(prop => prop.HasAttribute<RequiredAttribute>())
                .Where(prop => !prop.HasAttribute<ForeignKeyAttribute>())
                .Where(prop => prop.HasAttribute<IndexAttribute>())
                .Select(prop => new PropertyConstraint(this, prop.Name))
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
                                .Equals(this.GetFieldValue(dependencyColumn.ColumnName));
                        }

                        return dependentColumnIsValid;
                    })
                    .Select(prop =>
                    {
                        List<PropertyConstraint> subPropertyConstraints = new List<PropertyConstraint>();

                        ForeignKeyAttribute foreignKey = prop.GetCustomAttribute<ForeignKeyAttribute>();

                        Entity subEntity = (Entity)this.GetFieldValue(foreignKey.Name);

                        return subEntity.GetType().GetProperties()
                            .Where(subProp => !subProp.Name.Equals(nameof(this.Id)))
                            .Where(subProp => subProp.HasAttribute<ColumnAttribute>())
                            .Where(subProp => subProp.HasAttribute<RequiredAttribute>())
                            .Where(subProp => subProp.HasAttribute<IndexAttribute>())
                            .Where(subProp => !subProp.HasAttribute<ForeignKeyAttribute>())
                            .Select(subProp => new PropertyConstraint(subEntity, subProp.Name, foreignKey.Name));
                    })
                    .SelectMany(prop => prop)
                    .ToList().ForEach(constraints.Add);
            }

            return constraints;
        }

        public EnumContentType GetContentType(string columnName)
        {
            EnumContentType contentType;

            object value = this.GetFieldValue(columnName);

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
