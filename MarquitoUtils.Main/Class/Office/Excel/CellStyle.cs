using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarquitoUtils.Main.Class.Office.Excel
{
    public sealed class CellStyle
    {
        public bool IsLocked = false;
        public bool IsHidden = false;
        public CellStyle()
        {
        }

        public ICellStyle GetCellStyle(IWorkbook xWorkBook)
        {
            ICellStyle cellStyle = xWorkBook.CreateCellStyle();

            cellStyle.IsLocked = this.IsLocked;
            cellStyle.IsHidden = this.IsHidden;

            return cellStyle;
        }
    }
}
