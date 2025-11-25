using MarquitoUtils.Main.Common.Enums;
using MarquitoUtils.Main.Common.Tools;
using MarquitoUtils.Main.Office.Excel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarquitoUtils.Main.Office.Excel.Import
{
    /// <summary>
    /// Import with Excel files
    /// </summary>
    public abstract class ImportExcel : ImportExportExcel
    {
        /// <summary>
        /// Import with Excel files
        /// </summary>
        /// <param name="fileName">The Excel's file to import</param>
        protected ImportExcel(string fileName) : base(fileName)
        {
        }

        /// <summary>
        /// Init the import of the excel's file
        /// </summary>
        public void Init()
        {
            ManageData();

            Load();
            ImportData();

            ManageDataRows();
        }

        /// <summary>
        /// Manage data and create sheets, columns and rows
        /// </summary>
        private void ManageData()
        {
            ManageSheets();
            ManageColumns();
        }

        /// <summary>
        /// Import data
        /// </summary>
        private void ImportData()
        {
            this.ImportSheets();
        }

        /// <summary>
        /// Import sheets
        /// </summary>
        private void ImportSheets()
        {
            this.WorkBook.Sheets.ToList().ForEach(sheet =>
            {
                ISheet xSheet = this.XWorkBook.GetSheet(sheet.Name);

                this.ImportDataRows(sheet, xSheet);
            });
        }

        /// <summary>
        /// Import data's rows
        /// </summary>
        /// <param name="sheet">The sheet</param>
        /// <param name="xSheet">The real sheet</param>
        private void ImportDataRows(ExcelSheet sheet, ISheet xSheet)
        {
            int rowCount = sheet.HeaderRowNumber + 1;

            IRow xRow = xSheet.GetRow(rowCount);

            while (Utils.IsNotNull(xRow))
            {
                if (xRow.Cells.All(cell => Utils.IsEmpty(cell.StringCellValue)))
                {
                    xRow = null;
                }
                else
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
        }

        /// <summary>
        /// Set cell value
        /// </summary>
        /// <param name="cell">The cell</param>
        /// <param name="xCell">The real cell</param>
        private void SetCellValue(ExcelCell cell, ICell xCell)
        {
            switch (cell.ValueType)
            {
                case EnumContentType.Date:
                    if (!xCell.DateCellValue.Equals(new DateTime(1, 1, 1, 0, 0, 0)))
                    {
                        cell.Value = xCell.DateCellValue;
                    }
                    break;
                case EnumContentType.Number:
                    cell.Value = xCell.NumericCellValue;
                    break;
                case EnumContentType.Boolean:
                    cell.Value = xCell.BooleanCellValue;
                    break;
                case EnumContentType.Text:
                default:
                    if (xCell.CellType.Equals(CellType.Numeric))
                    {
                        cell.Value = Utils.GetAsString(xCell.NumericCellValue);
                    }
                    else
                    {
                        cell.Value = xCell.StringCellValue;
                    }
                    break;
            }
        }

        /// <summary>
        /// Load the workbook
        /// </summary>
        private void Load()
        {
            using (FileStream fs = new FileStream(WorkBook.Filename, FileMode.Open, FileAccess.Read))
            {
                XWorkBook = new XSSFWorkbook(fs);
            }
        }
    }
}
