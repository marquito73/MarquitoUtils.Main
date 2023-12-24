using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace MarquitoUtils.Main.Class.Office.Excel
{
    /// <summary>
    /// Import / export with Excel files
    /// </summary>
    public abstract class ImportExportExcel
    {
        /// <summary>
        /// The excel workbook
        /// </summary>
        protected ExcelWorkBook WorkBook { get; set; }
        /// <summary>
        /// The excel workbook from NPOI
        /// </summary>
        protected IWorkbook XWorkBook { get; set; } = new XSSFWorkbook();

        /// <summary>
        /// Import / export with Excel files
        /// </summary>
        /// <param name="fileName">The filename of the Excel workbook</param>
        public ImportExportExcel(string fileName)
        {
            WorkBook = new ExcelWorkBook(fileName);
        }

        /// <summary>
        /// Manage workbook's sheets here
        /// </summary>
        protected abstract void ManageSheets();

        /// <summary>
        /// Manage workbook's columns here
        /// </summary>
        protected abstract void ManageColumns();

        /// <summary>
        /// Manage workbook's rows here
        /// </summary>
        protected abstract void ManageDataRows();

        /// <summary>
        /// Get an existing sheet with this name (or a new one if not found)
        /// </summary>
        /// <param name="sheetName">The sheet's name</param>
        /// <returns>An existing sheet with this name (or a new one if not found)</returns>
        protected ExcelSheet GetSheet(string sheetName)
        {
            return WorkBook.GetSheet(sheetName);
        }
    }
}
