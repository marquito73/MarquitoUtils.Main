using MarquitoUtils.Main.Class.Entities.Sql;
using MarquitoUtils.Main.Class.Enums;
using MarquitoUtils.Main.Class.Office.Excel.Tools;
using MarquitoUtils.Main.Class.Service.Sql;
using MarquitoUtils.Main.Class.Sql;
using MarquitoUtils.Main.Class.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MarquitoUtils.Main.Class.Office.Excel.Import
{
    public class ImportExcelEntities<DBContext> : ImportExcel
        where DBContext : DefaultDbContext
    {
        private IEntityService EntityService { get; set; } = new EntityService();

        public ImportExcelEntities(string fileName, DBContext dbContext) : base(fileName)
        {
            this.EntityService.DbContext = dbContext;
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

                            if (property.PropertyType.IsDateTimeType() && property.PropertyType.IsNullableType())
                            {
                                entity.SetFieldValue(property.Name, (DateTime?)cell.Value);
                            }
                            else
                            {
                                entity.SetFieldValue(property.Name, Convert.ChangeType(cell.Value, property.PropertyType));
                            }
                        }
                        else
                        {
                            /*PropertyInfo subEntityProperty = entityType.GetFirstPropertyOfType(property.DeclaringType);

                            cell.Value = ((Entity)entity.GetFieldValue(subEntityProperty.Name)).GetFieldValue(property.Name);*/
                        }
                        columnCount++;
                    }

                    //entity.SetFieldValue
                });
            }
        }
    }
}
