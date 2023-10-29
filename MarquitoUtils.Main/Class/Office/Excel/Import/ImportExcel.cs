using MarquitoUtils.Main.Class.Enums;
using MarquitoUtils.Main.Class.Tools;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarquitoUtils.Main.Class.Office.Excel.Import
{
    public abstract class ImportExcel : ImportExportExcel
    {
        protected ImportExcel(string fileName) : base(fileName)
        {
        }

        public void Init()
        {
            ManageData();

            Load();
            ImportData();

            ManageDataRows();
        }

        private void ManageData()
        {
            ManageSheets();
            ManageColumns();
        }

        private void ImportData()
        {
            this.ImportSheets();
        }

        private void ImportSheets()
        {
            this.WorkBook.Sheets.ToList().ForEach(sheet =>
            {
                ISheet xSheet = this.XWorkBook.GetSheet(sheet.Name);

                this.ImportDataRows(sheet, xSheet);
            });
        }

        private void ImportDataRows(ExcelSheet sheet, ISheet xSheet)
        {
            int rowCount = sheet.HeaderRowNumber + 1;

            IRow xRow = xSheet.GetRow(rowCount);

            while (Utils.IsNotNull(xRow))
            {
                ExcelRow row = sheet.GetRow(rowCount);

                sheet.Columns.ToList().ForEach(column =>
                {
                    ICell xCell = xRow.GetCell(column.ColumnNumber);

                    ExcelCell cell = row.GetCell(column);

                    this.SetCellValue(cell, xCell);
                });

                rowCount++;

                xRow = xSheet.GetRow(rowCount);
            }
        }

        private void SetCellValue(ExcelCell cell, ICell xCell)
        {
            switch (cell.ValueType)
            {
                case EnumContentType.Date:
                    cell.Value = xCell.DateCellValue;
                    break;
                case EnumContentType.Number:
                    cell.Value = xCell.NumericCellValue;
                    break;
                case EnumContentType.Boolean:
                    cell.Value = xCell.BooleanCellValue;
                    break;
                case EnumContentType.Text:
                default:
                    cell.Value = xCell.StringCellValue;
                    break;
            }
        }

        private void Load()
        {
            using (FileStream fs = new FileStream(WorkBook.Filename, FileMode.Open, FileAccess.Read))
            {
                XWorkBook = new XSSFWorkbook(fs);
            }
        }
    }
}
