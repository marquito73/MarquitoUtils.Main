using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarquitoUtils.Main.Class.Office.Excel
{
    public abstract class ImportExportExcel
    {
        protected ExcelWorkBook WorkBook { get; set; }
        protected IWorkbook XWorkBook { get; set; } = new XSSFWorkbook();

        public ImportExportExcel(string fileName)
        {
            WorkBook = new ExcelWorkBook(fileName);
        }

        protected abstract void ManageSheets();

        protected abstract void ManageColumns();

        protected abstract void ManageDataRows();

        protected ExcelSheet GetSheet(string sheetName)
        {
            return WorkBook.GetSheet(sheetName);
        }
    }
}
