using MarquitoUtils.Main.Common.Enums;
using MarquitoUtils.Main.Common.Tools;
using MarquitoUtils.Main.Sql.Attributes;
using MarquitoUtils.Main.Sql.Context;
using MarquitoUtils.Main.Sql.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace MarquitoUtils.Main.Office.Excel.Tools
{
    /// <summary>
    /// Excel utils functions
    /// </summary>
    public static class ExcelUtils
    {
        /// <summary>
        /// Get entity types to import or export
        /// </summary>
        /// <typeparam name="DBContext">The database's context</typeparam>
        /// <returns>Entity types to import or export</returns>
        public static ISet<Type> GetEntityTypesToImportExport<DBContext>()
            where DBContext : DefaultDbContext
        {
            return typeof(DBContext).GetProperties()
                .Where(prop => prop.PropertyType.IsGenericDbSetType())
                .Select(prop => prop.PropertyType.GenericTypeArguments[0])
                .Where(entityType =>
                {
                    CanBeExportedImportedAttribute? canBeExportedImported = entityType
                    .GetCustomAttribute<CanBeExportedImportedAttribute>();

                    return Utils.IsNotNull(canBeExportedImported) && canBeExportedImported.CanBeExportedImported;
                })
                .ToHashSet();
        }

        /// <summary>
        /// Get entity's properties
        /// </summary>
        /// <param name="entityType">The entity type</param>
        /// <returns>Entity's properties</returns>
        public static ISet<PropertyInfo> GetEntityProperties(Type entityType)
        {
            IEnumerable<PropertyInfo> properties = entityType.GetProperties()
                // Exclude id
                .Where(prop => !prop.Name.Equals(nameof(Entity.Id)))
                // Exclude entity code
                .Where(prop => !prop.Name.Equals(nameof(Entity.EntityIdentityCode)))
                // Exclude list of entities
                .Where(prop => !prop.PropertyType.IsGenericCollectionType()
                || !prop.PropertyType.GenericTypeArguments[0].IsAnEntityType())
                .Where(prop => !prop.HasAttribute<ForeignKeyAttribute>());
            // First get all fields without entity
            List<PropertyInfo> entityProperties = new List<PropertyInfo>();
            // First, get required field for retrieve a sub entity
            properties.Where(prop => prop.PropertyType.IsAnEntityType())
                .ToList().ForEach(prop =>
                {
                    IEnumerable<IGrouping<string, PropertyInfo>> subProperties =
                    prop.PropertyType.GetProperties()
                    .Where(subProp => !subProp.Name.Equals(nameof(Entity.Id)))
                    .Where(subProp =>
                    {
                        IndexAttribute? index = subProp.GetIndexAttribute();

                        return Utils.IsNotNull(index) && index.IsUnique;
                    })
                    .GroupBy(subProp => subProp.GetCustomAttribute<IndexAttribute>().Name);
                    if (subProperties.Any())
                    {
                        subProperties.First().ToList().ForEach(subProp =>
                        {
                            entityProperties.Add(subProp);
                        });
                    }
                });
            // And next, get all fields without entity
            entityProperties.AddRange(properties
                .Where(prop => !prop.PropertyType.IsAnEntityType()));

            return entityProperties.OrderBy(prop => IsDependencyColumn(prop) ? 0 : 1).ToHashSet();
        }

        /// <summary>
        /// The property is a dependency's column ?
        /// </summary>
        /// <param name="property">The property</param>
        /// <returns>The property is a dependency's column ?</returns>
        private static bool IsDependencyColumn(PropertyInfo property)
        {
            return property.DeclaringType.GetProperties()
                .Where(prop => prop.HasAttribute<DependencyColumnAttribute>())
                .Where(prop => prop.GetCustomAttribute<DependencyColumnAttribute>().ColumnName.Equals(property.Name))
                .Any();
        }

        /// <summary>
        /// Get entity properties as a Map
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <returns>Entity properties as a Map</returns>
        public static IDictionary<Type, ISet<PropertyInfo>> GetEntityPropertiesMap(Entity entity)
        {
            Type entityType = entity.GetType();

            return entityType.GetProperties()
                // Exclude id
                .Where(prop => !prop.Name.Equals(nameof(Entity.Id)))
                // Exclude entity code
                .Where(prop => !prop.Name.Equals(nameof(Entity.EntityIdentityCode)))
                // Exclude list of entities
                .Where(prop => !prop.PropertyType.IsGenericCollectionType()
                || !prop.PropertyType.GenericTypeArguments[0].IsAnEntityType())
                .Where(prop => !prop.HasAttribute<ForeignKeyAttribute>())
                .Where(prop => prop.PropertyType.IsAnEntityType())
                .Where(prop =>
                {
                    bool dependentColumnIsValid = true;

                    if (prop.HasAttribute<DependencyColumnAttribute>())
                    {
                        DependencyColumnAttribute dependencyColumn = prop.GetCustomAttribute<DependencyColumnAttribute>();

                        dependentColumnIsValid = dependencyColumn.DependentValue
                            .Equals(entity.GetFieldValue<object>(dependencyColumn.ColumnName));
                    }

                    return dependentColumnIsValid;
                })
                .GroupBy(prop => prop.DeclaringType).
                ToDictionary(x => x.Key, x => (ISet<PropertyInfo>)x.ToHashSet());
        }

        /// <summary>
        /// Get value type for a property
        /// </summary>
        /// <param name="property">The property</param>
        /// <returns>Value type for a property</returns>
        public static EnumContentType GetValueType(PropertyInfo property)
        {
            EnumContentType contentType;

            if (property.PropertyType.IsDateTimeType())
            {
                contentType = EnumContentType.Date;
            }
            else if (property.PropertyType.IsEnum)
            {
                contentType = EnumContentType.Text;
            }
            else if (property.PropertyType.IsNumericType())
            {
                contentType = EnumContentType.Number;
            }
            else if (property.PropertyType.Equals(typeof(bool)))
            {
                contentType = EnumContentType.Boolean;
            }
            else
            {
                contentType = EnumContentType.Text;
            }

            return contentType;
        }
    }
}
