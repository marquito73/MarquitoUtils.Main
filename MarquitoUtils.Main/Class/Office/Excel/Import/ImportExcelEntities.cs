using MarquitoUtils.Main.Class.Entities.Sql;
using MarquitoUtils.Main.Class.Entities.Sql.Attributes;
using MarquitoUtils.Main.Class.Office.Excel.Tools;
using MarquitoUtils.Main.Class.Service.Import;
using MarquitoUtils.Main.Class.Sql;
using MarquitoUtils.Main.Class.Tools;
using System.Reflection;

namespace MarquitoUtils.Main.Class.Office.Excel.Import
{
    /// <summary>
    /// Import entities with Excel files
    /// </summary>
    /// <typeparam name="DBContext">The database's context</typeparam>
    public class ImportExcelEntities<DBContext> : ImportExcel
        where DBContext : DefaultDbContext
    {
        private IImportService ImportService { get; set; } = new ImportService();

        /// <summary>
        /// Import entities with Excel files
        /// </summary>
        /// <param name="fileName">The Excel's file to import</param>
        /// <param name="dbContext">The database's context</param>
        public ImportExcelEntities(string fileName, DBContext dbContext) : base(fileName)
        {
            this.ImportService.DbContext = dbContext;
        }

        protected override void ManageSheets()
        {
            foreach (Type entityType in ExcelUtils.GetEntityTypesToImportExport<DBContext>())
            {
                this.GetSheet(entityType.Name);
            }
        }

        protected override void ManageColumns()
        {
            foreach (Type entityType in ExcelUtils.GetEntityTypesToImportExport<DBContext>())
            {
                ExcelSheet sheet = this.GetSheet(entityType.Name);

                int count = 0;
                foreach (PropertyInfo property in ExcelUtils.GetEntityProperties(entityType))
                {
                    ExcelColumn column = sheet.GetColumn(count);

                    if (property.Name.Equals(nameof(Entity.Id)))
                    {
                        column.CellStyle.IsHidden = true;
                        column.CellStyle.IsLocked = true;
                    }

                    column.ValueType = ExcelUtils.GetValueType(property);
                    column.Name = property.Name;
                    count++;
                }
            }
        }

        protected override void ManageDataRows()
        {
            List<Entity> entitiesToImport = new List<Entity>();

            foreach (Type entityType in ExcelUtils.GetEntityTypesToImportExport<DBContext>())
            {
                ExcelSheet sheet = this.GetSheet(entityType.Name);

                sheet.Rows.ToList().ForEach(row =>
                {
                    Entity entity = (Entity)Activator.CreateInstance(entityType);

                    int columnCount = 0;

                    foreach (PropertyInfo property in ExcelUtils.GetEntityProperties(entityType))
                    {
                        ExcelColumn column = sheet.GetColumn(columnCount);

                        ExcelCell cell = row.GetCell(column);

                        if (entityType.Equals(property.DeclaringType) || entityType.IsSubclassOf(property.DeclaringType))
                        {
                            this.SetSimpleProperty(property, entity, cell);
                        }
                        else
                        {
                            this.SetEntityProperty(property, entity, cell);
                        }
                        columnCount++;
                    }

                    foreach (KeyValuePair<Type, ISet<PropertyInfo>> entityProperties in ExcelUtils.GetEntityPropertiesMap(
                        entity))
                    {
                        foreach (PropertyInfo entityProperty in entityProperties.Value)
                        {
                            Entity subEntity = (Entity)entity.GetFieldValue<object>(entityProperty.Name);

                            if (Utils.IsNotNull(subEntity))
                            {
                                IEnumerable<PropertyConstraint<Entity>> constraints = subEntity.GetPropertyConstraints(false);

                                Entity entityFound = this.ImportService.FindEntityByUniqueConstraint(constraints.ToList(),
                                    subEntity.GetType());

                                if (Utils.IsNotNull(entityFound))
                                {
                                    entity.SetFieldValue(entityProperty.Name, entityFound);
                                }
                                else
                                {
                                    // Throw Exception, the sub entity need to exist
                                    throw new Exception("The sub entity need to exist");
                                }
                            }
                        }
                    }

                    entitiesToImport.Add(entity);
                });
            }

            if (Utils.IsNotEmpty(entitiesToImport))
            {
                entitiesToImport.ForEach(this.ImportEntity);

                this.ImportService.FlushData(out Exception exception);
            }
        }

        /// <summary>
        /// Import an entity
        /// </summary>
        /// <param name="entityToImport">The entity to import</param>
        private void ImportEntity(Entity entityToImport)
        {
            IEnumerable<PropertyConstraint<Entity>> constraints = entityToImport.GetPropertyConstraints();
            // The entity found with constraints
            Entity entityFound = this.ImportService.FindEntityByUniqueConstraint(constraints.ToList(), 
                entityToImport.GetType());

            if (Utils.IsNull(entityFound))
            {
                // Entity doesnt exist, we need to insert
                entityToImport.Id = 0;
                this.ImportService.PersistEntity(entityToImport);
            }
            else
            {
                // Entity exist, we need to update
                foreach (PropertyInfo property in ExcelUtils.GetEntityProperties(entityFound.GetType()))
                {
                    if (entityFound.GetType().Equals(property.DeclaringType) 
                        || entityFound.GetType().IsSubclassOf(property.DeclaringType))
                    {
                        this.SetSimpleProperty(property, entityFound, entityToImport.GetFieldValue<object>(property.Name));
                    }
                    else
                    {
                        PropertyInfo firstEntityProperty = entityToImport.GetType().GetProperties()
                            .Where(prop => prop.PropertyType.Equals(property.DeclaringType))
                            .FirstOrDefault();

                        if (this.DependentColumnIsValid(entityToImport, firstEntityProperty))
                        {
                            Entity subEntity = this.GetFirstEntityPropertyOfType(entityToImport, property.DeclaringType);

                            this.SetEntityProperty(property, entityFound,
                                subEntity.GetFieldValue<object>(property.Name));
                        }
                    }
                }
            }
        }

        #region Set a property

        /// <summary>
        /// Set a simple property
        /// </summary>
        /// <param name="property">The property</param>
        /// <param name="entity">The entity which will have the value</param>
        /// <param name="cell">The cell that contains the value</param>
        private void SetSimpleProperty(PropertyInfo property, Entity entity, ExcelCell cell)
        {
            this.SetSimpleProperty(property, entity, cell.Value);
        }

        /// <summary>
        /// Set a simple property
        /// </summary>
        /// <param name="property">The property</param>
        /// <param name="entity">The entity which will have the value</param>
        /// <param name="value">The value</param>
        private void SetSimpleProperty(PropertyInfo property, Entity entity, object value)
        {
            object fieldValue = entity.GetFieldValue<object>(property.Name);
            if (Utils.IsNull(fieldValue) || !fieldValue.Equals(value))
            {
                if (property.PropertyType.IsDateTimeType() && property.PropertyType.IsNullableType())
                {
                    entity.SetFieldValue(property.Name, (DateTime?)value);
                }
                else if (property.PropertyType.IsEnum)
                {
                    entity.SetFieldValue(property.Name, Enum.Parse(property.PropertyType,
                        Utils.GetAsString(value)));
                }
                else
                {
                    entity.SetFieldValue(property.Name, Convert.ChangeType(value, property.PropertyType));
                }
            }
        }

        /// <summary>
        /// Set a simple property of a sub entity
        /// </summary>
        /// <param name="property">The property</param>
        /// <param name="entityType">The sub entity type</param>
        /// <param name="entity">The entity which will have the value</param>
        /// <param name="cell">The cell that contains the value</param>
        private void SetEntityProperty(PropertyInfo property, Entity entity, ExcelCell cell)
        {
            this.SetEntityProperty(property, entity, cell.Value);
        }

        /// <summary>
        /// Set a simple property of a sub entity
        /// </summary>
        /// <param name="property">The property</param>
        /// <param name="entityType">The sub entity type</param>
        /// <param name="entity">The entity which will have the value</param>
        /// <param name="value">The value</param>
        private void SetEntityProperty(PropertyInfo property, Entity entity, object value)
        {
            PropertyInfo firstEntityProperty = entity.GetType().GetProperties()
                .Where(prop => prop.PropertyType.Equals(property.DeclaringType))
                .FirstOrDefault();

            if (Utils.IsNotNull(firstEntityProperty))
            {
                if (this.DependentColumnIsValid(entity, firstEntityProperty))
                {
                    Entity subEntity = entity.GetFieldValue<Entity>(firstEntityProperty.Name);

                    if (Utils.IsNull(subEntity))
                    {
                        entity.SetFieldValue(firstEntityProperty.Name, Activator.CreateInstance(firstEntityProperty.PropertyType));

                        subEntity = entity.GetFieldValue<Entity>(firstEntityProperty.Name);
                    }

                    object fieldValue = subEntity.GetFieldValue<object>(property.Name);

                    if (Utils.IsNull(fieldValue) || !fieldValue.Equals(value))
                    {
                        subEntity.SetFieldValue(property.Name, Convert.ChangeType(value, property.PropertyType));
                    }
                }
            }
        }

        /// <summary>
        /// Dependent column is valid ?
        /// </summary>
        /// <param name="entity">The entity who has the property</param>
        /// <param name="prop">The dependency property</param>
        /// <returns>Dependent column is valid ?</returns>
        private bool DependentColumnIsValid(Entity entity, PropertyInfo prop)
        {
            bool dependentColumnIsValid = true;

            if (prop.HasAttribute<DependencyColumnAttribute>())
            {
                DependencyColumnAttribute dependencyColumn = prop.GetCustomAttribute<DependencyColumnAttribute>();

                dependentColumnIsValid = dependencyColumn.DependentValue
                    .Equals(entity.GetFieldValue<object>(dependencyColumn.ColumnName));
            }

            return dependentColumnIsValid;
        }

        /// <summary>
        /// Get first entity's property of type
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <param name="entityType">The entity's property</param>
        /// <returns>First entity's property of type</returns>
        private Entity GetFirstEntityPropertyOfType(Entity entity, Type entityType)
        {
            PropertyInfo firstEntityProperty = entity.GetType().GetProperties()
                .Where(prop => prop.PropertyType.Equals(entityType))
                .FirstOrDefault();

            return entity.GetFieldValue<Entity>(firstEntityProperty.Name);
        }

        #endregion Set a property
    }
}
