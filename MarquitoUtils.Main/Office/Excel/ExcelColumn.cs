using MarquitoUtils.Main.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarquitoUtils.Main.Office.Excel
{
    public class ExcelColumn
    {
        public ExcelSheet ParentSheet { get; set; }
        public int ColumnNumber { get; set; }
        public string Name { get; set; }
        public EnumContentType ValueType { get; set; }
        public CellStyle CellStyle { get; private set; } = new CellStyle();

        public ExcelColumn(ExcelSheet parentSheet, int rowNumber)
        {
            this.ParentSheet = parentSheet;
            this.ColumnNumber = rowNumber;
        }
    }
}
