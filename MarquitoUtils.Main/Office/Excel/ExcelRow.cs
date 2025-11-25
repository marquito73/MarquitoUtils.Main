using MarquitoUtils.Main.Common.Enums;
using MarquitoUtils.Main.Common.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarquitoUtils.Main.Office.Excel
{
    public sealed class ExcelRow
    {
        public ExcelSheet ParentSheet { get; set; }
        public int RowNumber { get; set; }
        public ISet<ExcelCell> Cells { get; private set; } = new HashSet<ExcelCell>();

        public ExcelRow(ExcelSheet parentSheet, int rowNumber)
        {
            this.ParentSheet = parentSheet;
            this.RowNumber = rowNumber;
        }

        public ExcelCell GetCell(ExcelColumn column)
        {
            ExcelCell cell = this.Cells.Where(cell => cell.ColumnNumber == column.ColumnNumber).FirstOrDefault();

            if (Utils.IsNull(cell))
            {
                cell = new ExcelCell(this, column);
                this.Cells.Add(cell);
            }

            return cell;
        }

        public ExcelCell GetCell(int columnNumber, EnumContentType valueType)
        {
            ExcelCell cell = this.Cells.Where(cell => cell.ColumnNumber == columnNumber).FirstOrDefault();

            if (Utils.IsNull(cell))
            {
                cell = new ExcelCell(this, columnNumber, valueType);
                this.Cells.Add(cell);
            }

            return cell;
        }
    }
}
