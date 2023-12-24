using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarquitoUtils.Main.Class.Office.Excel
{
    /// <summary>
    /// Style of a cell
    /// </summary>
    public sealed class CellStyle
    {
        /// <summary>
        /// The cell is locked ?
        /// </summary>
        public bool IsLocked = false;
        /// <summary>
        /// The cell is hidden ?
        /// </summary>
        public bool IsHidden = false;
        /// <summary>
        /// The cell use date format ?
        /// </summary>
        public bool UseDateFormat = false;

        /// <summary>
        /// Style of a cell
        /// </summary>
        public CellStyle()
        {
        }

        /// <summary>
        /// Get cell style
        /// </summary>
        /// <param name="xWorkBook">The real worbook</param>
        /// <returns>The cell style</returns>
        public ICellStyle GetCellStyle(IWorkbook xWorkBook)
        {
            ICellStyle cellStyle = xWorkBook.CreateCellStyle();

            cellStyle.IsLocked = this.IsLocked;
            cellStyle.IsHidden = this.IsHidden;

            if (this.UseDateFormat)
            {
                cellStyle.DataFormat = xWorkBook.CreateDataFormat().GetFormat("dd/MM/yyyy");
            }

            return cellStyle;
        }
    }
}
