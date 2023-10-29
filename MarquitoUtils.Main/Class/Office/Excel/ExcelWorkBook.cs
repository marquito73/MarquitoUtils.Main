using MarquitoUtils.Main.Class.Enums;
using MarquitoUtils.Main.Class.Office.Excel.Attributes;
using MarquitoUtils.Main.Class.Office.Excel.Enums;
using MarquitoUtils.Main.Class.Tools;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarquitoUtils.Main.Class.Office.Excel
{
    public sealed class ExcelWorkBook
    {
        public string Filename { get; set; }
        public ISet<ExcelSheet> Sheets { get; set; } = new HashSet<ExcelSheet>();

        public ExcelWorkBook(string filename)
        {
            Filename = filename;
        }

        public ExcelSheet GetSheet(string sheetName)
        {
            ExcelSheet sheet = this.Sheets.Where(row => row.Name == sheetName).FirstOrDefault();

            if (Utils.IsNull(sheet))
            {
                sheet = new ExcelSheet(this, sheetName);
                this.Sheets.Add(sheet);
            }

            return sheet;
        }
    }
}
