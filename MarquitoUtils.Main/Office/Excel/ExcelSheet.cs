using MarquitoUtils.Main.Common.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarquitoUtils.Main.Office.Excel
{
    public sealed class ExcelSheet
    {
        public ExcelWorkBook ParentWorkBook { get; set; }
        public string Name { get; set; }
        public bool Protect { get; set; }
        public string ProtectPassword { get; set; }
        public int HeaderRowNumber { get; set; } = 2;
        public bool AutoSize { get; set; } = true;
        public ISet<ExcelRow> Rows { get; set; } = new HashSet<ExcelRow>();
        public ISet<ExcelColumn> Columns { get; set; } = new HashSet<ExcelColumn>();

        public ExcelSheet(ExcelWorkBook parentWorkBook, string sheetName)
        {
            this.ParentWorkBook = parentWorkBook;
            this.Name = sheetName;
        }

        public ExcelRow GetRow(int rowNumber)
        {
            ExcelRow row = this.Rows.Where(row => row.RowNumber == rowNumber).FirstOrDefault();

            if (Utils.IsNull(row))
            {
                row = new ExcelRow(this, rowNumber);
                this.Rows.Add(row);
            }

            return row;
        }

        public ExcelColumn GetColumn(int columnNumber)
        {
            ExcelColumn column = this.Columns.Where(column => column.ColumnNumber == columnNumber).FirstOrDefault();

            if (Utils.IsNull(column))
            {
                column = new ExcelColumn(this, columnNumber);
                this.Columns.Add(column);
            }

            return column;
        }
    }
}
