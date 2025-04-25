using MarquitoUtils.Main.Class.Attributes.Sql;
using MarquitoUtils.Main.Class.Entities.Sql;
using Microsoft.EntityFrameworkCore.Metadata;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;
using System.Text;

namespace MarquitoUtils.Main.Class.Tools.Sql
{
    /// <summary>
    /// Helper provide methods to build a SQL script for generate a table, in accordance with an Entity
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public sealed class EntitySqlScriptHelperTest<TEntity>
            where TEntity : Entity
    {
        private string RC { get; set; } = "\n";
        private string TAB { get; set; } = "\t";

        /// <summary>
        /// Render and return the script created
        /// </summary>
        /// <returns>The script created</returns>
        public string RenderEntitySqlScript()
        {
            StringBuilder entitySqlScript = new StringBuilder();

            entitySqlScript.Append($"create table {this.GetTableName()} ({this.RC}");
            // Render column properties
            typeof(TEntity).GetProperties().Where(x => x.HasAttribute<ColumnAttribute>())
                .ForEach(prop =>
                {
                    entitySqlScript.Append($"{this.TAB}{this.RenderColumnProperty(prop)}, {this.RC}");
                });
            // Render primary key constraint
            entitySqlScript.Append($"{this.TAB}{this.RenderPrimaryConstraint()}");
            // Render foreign keys constraint
            typeof(TEntity).GetProperties().Where(x => x.HasAttribute<ColumnAttribute>() && x.HasAttribute<ForeignKeyAttribute>())
                .ForEach(prop =>
                {
                    entitySqlScript.Append($", {RC}{this.TAB}{this.RenderForeignKeyConstraint(prop)}");
                });
            // Render composite keys constraint
            this.GetCompositeKeys().ForEach(x =>
            {
                entitySqlScript.Append($", {RC}{this.TAB}{this.RenderCompositeKeyConstraint(x.Key, x.Value)}");
            });

            entitySqlScript.Append($"{RC})");

            return entitySqlScript.ToString();
        }

        /// <summary>
        /// Get the entity table name
        /// </summary>
        /// <returns>Entity table name</returns>
        private string GetTableName()
        {
            return typeof(TEntity).GetCustomAttribute<TableAttribute>().Name;
        }

        /// <summary>
        /// Render a column in accordance with an Entity property
        /// </summary>
        /// <param name="columnProperty">An Entity property</param>
        /// <returns>A column in accordance with an Entity property</returns>
        private string RenderColumnProperty(PropertyInfo columnProperty)
        {
            StringBuilder propertySql = new StringBuilder();

            ColumnAttribute columnAttribute = columnProperty.GetCustomAttribute<ColumnAttribute>();
            KeyAttribute? keyAttribute = columnProperty.GetCustomAttribute<KeyAttribute>();
            RequiredAttribute? requiredAttribute = columnProperty.GetCustomAttribute<RequiredAttribute>();

            propertySql.Append($"{columnAttribute.Name} ");

            if (Utils.IsNotNull(keyAttribute) || columnAttribute.TypeName == "bigint")
            {
                propertySql.Append("bigint ");
            }
            else
            {
                if (columnAttribute.TypeName == "nvarchar")
                {
                    MaxLengthAttribute maxLength = columnProperty.GetCustomAttribute<MaxLengthAttribute>();
                    propertySql.Append($"{columnAttribute.TypeName}({maxLength.Length}) ");
                }
                else
                {
                    propertySql.Append($"{columnAttribute.TypeName} ");
                }
            }

            if (Utils.IsNotNull(requiredAttribute))
            {
                propertySql.Append("not null");
            }
            else
            {
                propertySql.Append("default null");
            }

            return propertySql.ToString();
        }

        /// <summary>
        /// Render the primary key constraint of an Entity
        /// </summary>
        /// <returns>The primary key constraint of an Entity</returns>
        private string RenderPrimaryConstraint()
        {
            string primaryKeyConstraintSql = "";

            // Create primary key constraint
            PropertyInfo? property = typeof(TEntity).GetProperties()
                .Where(x => x.HasAttribute<ColumnAttribute>() && x.HasAttribute<KeyAttribute>())
                .FirstOrDefault();

            if (Utils.IsNotNull(property))
            {
                ColumnAttribute column = property.GetCustomAttribute<ColumnAttribute>();
                IndexAttribute index = property.GetCustomAttribute<IndexAttribute>();

                primaryKeyConstraintSql = $"constraint {index.Name} primary key clustered ({column.Name})";
            }

            return primaryKeyConstraintSql;
        }

        /// <summary>
        /// Render a foreign key constraint of an Entity
        /// </summary>
        /// <param name="property"></param>
        /// <returns>The foreign key constraint of an Entity</returns>
        private string RenderForeignKeyConstraint(PropertyInfo property)
        {
            ColumnAttribute column = property.GetCustomAttribute<ColumnAttribute>();
            ForeignKeyAttribute foreignKey = property.GetCustomAttribute<ForeignKeyAttribute>();

            TableAttribute foreignTableName = typeof(TEntity).GetProperty(foreignKey.Name).PropertyType.GetCustomAttribute<TableAttribute>();

            return $"constraint fk_{this.GetTableName()}_{foreignTableName.Name} foreign key ({column.Name}) references {foreignTableName.Name}({column.Name})";
        }

        /// <summary>
        /// Render a composite key constraint of an Entity
        /// </summary>
        /// <param name="compositeKeyName">The composite key constraint name</param>
        /// <param name="properties">A list of properties of the same composite key</param>
        /// <returns>The composite key constraint of an Entity</returns>
        private string RenderCompositeKeyConstraint(string compositeKeyName, List<PropertyInfo> properties)
        {
            StringBuilder compositeKeySql = new StringBuilder();

            compositeKeySql.Append($"constraint {compositeKeyName} unique(")
                .Append(properties.Select(property => property.GetCustomAttribute<ColumnAttribute>().Name).Join(", "))
                .Append(")");

            return compositeKeySql.ToString();
        }

        /// <summary>
        /// Get the list of composite key constraints of this Entity
        /// </summary>
        /// <returns>The list of composite key constraints of this Entity</returns>
        private Dictionary<string, List<PropertyInfo>> GetCompositeKeys()
        {
            return typeof(TEntity).GetProperties()
                .Where(x =>
                {
                    IndexAttribute? index = x.GetCustomAttribute<IndexAttribute>();

                    return Utils.IsNotNull(index) && index.IsUnique && index.Order > 0;
                }).GroupBy(x => x.GetCustomAttribute<IndexAttribute>().Name)
                .ToDictionary(x => x.Key, x => x.ToList());
        }
    }
}
