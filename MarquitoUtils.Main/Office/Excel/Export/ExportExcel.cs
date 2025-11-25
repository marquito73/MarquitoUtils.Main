using EnumsNET;
using MarquitoUtils.Main.Common.Enums;
using MarquitoUtils.Main.Common.Tools;
using MarquitoUtils.Main.Office.Excel;
using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using ICell = NPOI.SS.UserModel.ICell;

namespace MarquitoUtils.Main.Office.Excel.Export
{
    /// <summary>
    /// Export with Excel files
    /// </summary>
    public abstract class ExportExcel : ImportExportExcel
    {
        public uint OtherGridRowsIfSheetLocked { get; set; } = 50;
        protected ExportExcel(string fileName) : base(fileName)
        {
        }

        /// <summary>
        /// Init the export of the excel's file
        /// </summary>
        public void Init()
        {
            this.ManageData();

            this.CreateData();
        }

        /// <summary>
        /// Manage data and create sheets, columns and rows
        /// </summary>
        private void ManageData()
        {
            this.ManageSheets();
            this.ManageColumns();
            this.ManageDataRows();
        }

        /// <summary>
        /// Save data as workbook
        /// </summary>
        public void Save()
        {
            using (FileStream fs = new FileStream(this.WorkBook.Filename, FileMode.Create))
            {
                this.XWorkBook.Write(fs);
            }
        }

        /// <summary>
        /// Convert workbook into real workbook
        /// </summary>
        private void CreateData()
        {
            this.CreateSheets();
        }

        /// <summary>
        /// Convert sheets into real sheets
        /// </summary>
        private void CreateSheets()
        {
            uint sheetIndex = 1;
            this.WorkBook.Sheets.ToList().ForEach(sheet =>
            {
                ISheet xSheet = this.XWorkBook.CreateSheet(sheet.Name);

                if (sheet.Protect)
                {
                    xSheet.ProtectSheet("test");
                }

                this.CreateColumns(sheet, xSheet);
                this.CreateDataRows(sheet, xSheet);


                this.ApplyStyleToColumn(sheet, xSheet);

                this.ApplyTableStyle(sheet, xSheet, sheetIndex);

                sheetIndex++;
            });
        }

        /// <summary>
        /// Convert columns into real columns
        /// </summary>
        /// <param name="sheet">The sheet</param>
        /// <param name="xSheet">The real sheet</param>
        private void CreateColumns(ExcelSheet sheet, ISheet xSheet)
        {
            IRow headerRow = xSheet.CreateRow(sheet.HeaderRowNumber);
            sheet.Columns.ToList().ForEach(column =>
            {
                ICell headerCell = headerRow.CreateCell(column.ColumnNumber);
                headerCell.SetCellValue(column.Name);

            });
        }

        /// <summary>
        /// Apply style to a column
        /// </summary>
        /// <param name="sheet">The sheet</param>
        /// <param name="xSheet">The real sheet</param>
        private void ApplyStyleToColumn(ExcelSheet sheet, ISheet xSheet)
        {
            foreach (ExcelColumn column in sheet.Columns)
            {
                if (sheet.AutoSize && !column.CellStyle.IsHidden)
                {
                    xSheet.AutoSizeColumn(column.ColumnNumber);
                }
            }
        }

        /// <summary>
        /// Add cell's values to data rows
        /// </summary>
        /// <param name="sheet">The sheet</param>
        /// <param name="xSheet">The real sheet</param>
        private void CreateDataRows(ExcelSheet sheet, ISheet xSheet)
        {
            sheet.Rows.ToList().ForEach(row =>
            {
                IRow dataRow = xSheet.CreateRow(row.RowNumber + sheet.HeaderRowNumber + 1);

                row.Cells.ToList().ForEach(cell =>
                {
                    ICell dataCell = dataRow.CreateCell(cell.ColumnNumber);
                    //dataCell.SetCellValue(Utils.GetAsString(cell.Value));

                    if (!cell.ValueType.Equals(EnumContentType.Date))
                    {
                        dataCell.SetCellType(this.GetCellType(cell));
                    }

                    this.SetCellValue(cell, dataCell);

                    dataCell.CellStyle = this.MergeCellStyle(cell).GetCellStyle(this.XWorkBook);
                });
            });

            if (sheet.Protect)
            {
                // Unlock 50 rows for add possibility to import new rows
                for (int cpt = 1; cpt <= this.OtherGridRowsIfSheetLocked; cpt++)
                {
                    IRow dataRow = xSheet.CreateRow(sheet.Rows.Count + sheet.HeaderRowNumber + cpt);

                    sheet.Columns.ToList().ForEach(column =>
                    {
                        ICell dataCell = dataRow.CreateCell(column.ColumnNumber);

                        if (!column.ValueType.Equals(EnumContentType.Date))
                        {
                            dataCell.SetCellType(this.GetCellType(column));
                        }

                        dataCell.CellStyle = column.CellStyle.GetCellStyle(this.XWorkBook);
                    });
                }
            }
        }

        /// <summary>
        /// Get cell type for a column
        /// </summary>
        /// <param name="cell">The column</param>
        /// <returns>Cell type for a column</returns>
        private CellType GetCellType(ExcelColumn column)
        {
            return this.GetCellType(column.ValueType);
        }

        /// <summary>
        /// Get cell type for a cell
        /// </summary>
        /// <param name="cell">The cell</param>
        /// <returns>Cell type for a cell</returns>
        private CellType GetCellType(ExcelCell cell)
        {
            return this.GetCellType(cell.ValueType);
        }

        /// <summary>
        /// Get cell type
        /// </summary>
        /// <param name="dataCellType">The cell type</param>
        /// <returns>Cell type</returns>
        private CellType GetCellType(EnumContentType dataCellType)
        {
            CellType cellType;

            switch (dataCellType)
            {
                case EnumContentType.Date:
                    cellType = CellType.Blank;
                    break;
                case EnumContentType.Number:
                    cellType = CellType.Numeric;
                    break;
                case EnumContentType.Boolean:
                    cellType = CellType.Boolean;
                    break;
                case EnumContentType.Text:
                default:
                    cellType = CellType.String;
                    break;
            }

            return cellType;
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
                    if (Utils.IsNotNull(cell.Value))
                    {
                        xCell.SetCellValue((DateTime)cell.Value);
                    }
                    break;
                case EnumContentType.Number:
                    xCell.SetCellValue(Utils.GetAsDouble(cell.Value));
                    break;
                case EnumContentType.Boolean:
                    xCell.SetCellValue(Utils.GetAsBoolean(cell.Value));
                    break;
                case EnumContentType.Text:
                default:
                    xCell.SetCellValue(Utils.GetAsString(cell.Value));
                    break;
            }
        }

        /// <summary>
        /// Merge cell style, between column and cell
        /// </summary>
        /// <param name="cell">The cell</param>
        /// <returns>Merged cell style, between column and cell</returns>
        private CellStyle MergeCellStyle(ExcelCell cell)
        {
            CellStyle mergedStyle = new CellStyle();

            if (Utils.IsNotNull(cell.ParentColumn))
            {
                mergedStyle.IsLocked = cell.ParentColumn.CellStyle.IsLocked ? true : cell.CellStyle.IsLocked;
                mergedStyle.IsHidden = cell.ParentColumn.CellStyle.IsHidden ? true : cell.CellStyle.IsHidden;
            }
            else
            {
                mergedStyle = cell.CellStyle;
            }

            if (cell.ValueType.Equals(EnumContentType.Date))
            {
                mergedStyle.UseDateFormat = true;
            }

            return mergedStyle;
        }

        /// <summary>
        /// Apply style to table
        /// </summary>
        /// <param name="sheet">The sheet</param>
        /// <param name="xSheet">The real sheet</param>
        /// <param name="sheetIndex">The sheet's index</param>
        private void ApplyTableStyle(ExcelSheet sheet, ISheet xSheet, uint sheetIndex)
        {
            XSSFTable table = (xSheet as XSSFSheet).CreateTable();
            CT_Table ctTable = table.GetCTTable();

            XSSFBuiltinTableStyle test = new XSSFBuiltinTableStyle();

            int rowCount = sheet.Rows.Count == 0 ? 1 : sheet.Rows.Count;

            AreaReference dataRange = new AreaReference(new CellReference(sheet.HeaderRowNumber, 0), 
                new CellReference(sheet.HeaderRowNumber + rowCount + (int)this.OtherGridRowsIfSheetLocked, sheet.Columns.Count - 1));

            ctTable.@ref = dataRange.FormatAsString();
            ctTable.id = sheetIndex;
            ctTable.name = sheet.Name;
            ctTable.displayName = sheet.Name;
            ctTable.tableStyleInfo = new CT_TableStyleInfo();
            ctTable.tableStyleInfo.name = XSSFBuiltinTableStyleEnum.TableStyleMedium2.GetName();
            ctTable.tableStyleInfo.showRowStripes = true;
            ctTable.tableColumns = new CT_TableColumns();
            ctTable.tableColumns.tableColumn = new List<CT_TableColumn>();

            sheet.Columns.ToList().ForEach(column =>
            {
                ctTable.tableColumns.tableColumn.Add(new CT_TableColumn() 
                { 
                    id = (uint)column.ColumnNumber + 1, 
                    name = column.Name 
                });
            });
        }
    }
}
