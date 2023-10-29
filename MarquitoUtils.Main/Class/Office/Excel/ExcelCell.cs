using MarquitoUtils.Main.Class.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarquitoUtils.Main.Class.Office.Excel
{
    public sealed class ExcelCell
    {
        public ExcelRow ParentRow { get; set; }
        public ExcelColumn ParentColumn { get; set; }
        public int RowNumber { get; set; }
        public int ColumnNumber { get; set; }
        public object Value { get; set; }
        public EnumContentType ValueType { get; private set; }
        public CellStyle CellStyle { get; private set; } = new CellStyle();

        public ExcelCell(ExcelRow parentRow, int columnNumber, EnumContentType valueType)
        {
            this.ParentRow = parentRow;
            this.RowNumber = this.ParentRow.RowNumber;
            this.ColumnNumber = columnNumber;
            this.ValueType = valueType;
        }

        public ExcelCell(ExcelRow parentRow, ExcelColumn parentColumn)
        {
            this.ParentRow = parentRow;
            this.RowNumber = this.ParentRow.RowNumber;
            this.ParentColumn = parentColumn;
            this.ColumnNumber = this.ParentColumn.ColumnNumber;
            this.ValueType = parentColumn.ValueType;
        }
    }
}
