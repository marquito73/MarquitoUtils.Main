using MarquitoUtils.Main.Class.Entities.Sql;
using MarquitoUtils.Main.Class.Entities.Sql.Attributes;
using MarquitoUtils.Main.Class.Enums;
using MarquitoUtils.Main.Class.Sql;
using MarquitoUtils.Main.Class.Tools;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace MarquitoUtils.Main.Class.Office.Excel.Tools
{
    public static class ExcelUtils
    {
        public static ISet<Type> GetEntityTypesToImportExport<DBContext>()
            where DBContext : DefaultDbContext
        {
            return typeof(DBContext).GetProperties()
                .Where(prop => Utils.IsGenericDbSetType(prop.PropertyType))
                .Select(prop => prop.PropertyType.GenericTypeArguments[0])
                .Where(entityType =>
                {
                    CanBeExportedImportedAttribute? canBeExportedImported = entityType
                    .GetCustomAttribute<CanBeExportedImportedAttribute>();

                    return Utils.IsNotNull(canBeExportedImported) && canBeExportedImported.CanBeExportedImported;
                })
                .ToHashSet();
        }

        public static ISet<PropertyInfo> GetEntityProperties(Type entityType)
        {
            IEnumerable<PropertyInfo> properties = entityType.GetProperties()
                // Exclude list of entities
                .Where(prop => !Utils.IsGenericCollectionType(prop.PropertyType)
                || !Utils.IsAnEntityType(prop.PropertyType.GenericTypeArguments[0]))
                .Where(prop => !prop.HasAttribute<ForeignKeyAttribute>());
            // First get all fields without entity
            List<PropertyInfo> entityProperties = new List<PropertyInfo>();
            // First, get required field for retrieve a sub entity
            properties.Where(prop => Utils.IsAnEntityType(prop.PropertyType))
                .ToList().ForEach(prop =>
                {
                    IEnumerable<IGrouping<string, PropertyInfo>> subProperties =
                    prop.PropertyType.GetProperties()
                    .Where(subProp => !subProp.Name.Equals(nameof(Entity.Id)))
                    .Where(subProp =>
                    {
                        IndexAttribute? index = subProp.GetCustomAttribute<IndexAttribute>();

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
                .Where(prop => !Utils.IsAnEntityType(prop.PropertyType)));

            return entityProperties.ToHashSet();
        }

        public static EnumContentType GetValueType(PropertyInfo property)
        {
            EnumContentType contentType;

            if (property.PropertyType.IsDateTimeType())
            {
                contentType = EnumContentType.Date;
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
