using MarquitoUtils.Main.Class.Entities.Sql;
using MarquitoUtils.Main.Class.Office.Excel.Tools;
using MarquitoUtils.Main.Class.Service.Import;
using MarquitoUtils.Main.Class.Sql;
using MarquitoUtils.Main.Class.Tools;
using System.Reflection;

namespace MarquitoUtils.Main.Class.Office.Excel.Export
{
    /// <summary>
    /// Export database's entities with Excel's file
    /// </summary>
    /// <typeparam name="DBContext">The database's context</typeparam>
    public sealed class ExportExcelEntities<DBContext> : ExportExcel
        where DBContext : DefaultDbContext
    {
        /// <summary>
        /// The import's service
        /// </summary>
        private IImportService ImportService { get; set; } = new ImportService();

        /// <summary>
        /// Export database's entities with Excel's file
        /// </summary>
        /// <param name="fileName">Filename for the Excel export file</param>
        /// <param name="dbContext">The database's context</param>
        public ExportExcelEntities(string fileName, DBContext dbContext) : base(fileName)
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
                sheet.Protect = true;
                // TODO Find a better way, for tests only
                sheet.ProtectPassword = "Test";

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
            foreach (Type entityType in ExcelUtils.GetEntityTypesToImportExport<DBContext>())
            {
                ExcelSheet sheet = this.GetSheet(entityType.Name);

                int rowCount = 0;
                foreach (Entity entity in this.ImportService.GetEntityList(entityType))
                {
                    ExcelRow row = sheet.GetRow(rowCount);

                    int columnCount = 0;
                    foreach (PropertyInfo property in ExcelUtils.GetEntityProperties(entityType))
                    {
                        ExcelColumn column = sheet.GetColumn(columnCount);

                        ExcelCell cell = row.GetCell(column);

                        if (entityType.Equals(property.DeclaringType) || entityType.IsSubclassOf(property.DeclaringType))
                        {
                            cell.Value = entity.GetFieldValue<object>(property.Name);
                        }
                        else
                        {
                            PropertyInfo subEntityProperty = entityType.GetFirstPropertyOfType(property.DeclaringType);

                            Entity subEntity = ((Entity)entity.GetFieldValue<object>(subEntityProperty.Name));

                            if (Utils.IsNotNull(subEntity))
                            {
                                cell.Value = subEntity.GetFieldValue<object>(property.Name);
                            }
                        }

                        columnCount++;
                    }
                    rowCount++;
                }
            }
        }
    }
}
